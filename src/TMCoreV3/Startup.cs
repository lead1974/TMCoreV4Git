using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TMCoreV3.Services;
using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Models.User;
using Microsoft.AspNetCore.Http;
using TMCoreV3.DataAccess.Repos;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.ViewModels.CustomerViewModels;

namespace TMCoreV3
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<TMDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AuthUser, AuthRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";

                options.SignIn.RequireConfirmedEmail = true;
            })
              .AddEntityFrameworkStores<TMDbContext>()
              .AddDefaultTokenProviders();

            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();  /*new DefaultContractResolver();*/  //
                    config.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    
                });
            services.AddKendo();

            // Add application services.
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISmsService, MailService>();
            services.AddTransient<GlobalService, GlobalService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerApplianceTypeRepository, CustomerApplianceTypeRepository>();
            services.AddScoped<ICustomerApplianceBrandRepository, CustomerApplianceBrandRepository>();
            services.AddScoped<ICustomerCouponRepository, CustomerCouponRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<SelectedTabFilterAttribute>();

            services.AddTransient<TMCoreSeedData>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TMCoreSeedData seedData)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Mapper.Initialize(config =>
            {
                config.CreateMap<CustomerForm, Customer>().ReverseMap();
                config.CreateMap<ContactViewModel, Contact>().ReverseMap();
                config.CreateMap<ScheduleAppointment, Customer>().ReverseMap();
            });


            app.UseApplicationInsightsRequestTelemetry();
            app.UseExceptionHandler("/Home/Error");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                    throw new Exception("ERROR!");
                await next();
            });

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseIdentity();

            //app.UseStatusCodePagesWithRedirects("~/errors/{0}");
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
                //routes.MapRoute(
                //    name: "defaultApi",
                //    template: "api/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();

            //seeding Data on startup!
            seedData.EnsureSeedData().Wait();
        }
    }
}
