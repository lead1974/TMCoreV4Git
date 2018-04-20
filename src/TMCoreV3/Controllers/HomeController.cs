using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.Services;
using Microsoft.Extensions.Logging;
using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Repos;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using TMCoreV3.ViewModels.CustomerViewModels;
using AutoMapper;
using TMCoreV3.DataAccess.Models.Customer;

namespace TMCoreV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;
        private TMDbContext _TMDbContext;
        private ICustomerCouponRepository _customerCouponRepo;
        private readonly IHostingEnvironment _env;
        private IContactRepository _contactRepo;

        public HomeController(
            TMDbContext dmContext,
            ICustomerCouponRepository customerCouponRepo,
            IContactRepository contactRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env)
        {            
            _TMDbContext = dmContext;
            _customerCouponRepo = customerCouponRepo;
            _contactRepo = contactRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<HomeController>();
            _env = env;
        }

        [SelectedTabFilter("home")]
        public IActionResult Index()
        {
            ViewBag.SelectiveTab = "home";

            var customerCoupons = _customerCouponRepo.GetAllNonExpired().ToList();
            ViewBag.ShowCoupons = "no";
            if (customerCoupons.Count > 0) ViewBag.ShowCoupons = "yes";

            return View();
        }

        [SelectedTabFilter("about")]
        public IActionResult About()
        {
            ViewData["Message"] = "ATApplianceServiceINC ";
            ViewBag.SelectiveTab = "about";
            return View();
        }

        [SelectedTabFilter("contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Feel free contacting us anytime!";
            ViewBag.SelectiveTab = "contact";
            return View();
        }

        public IActionResult ContactEditingPopup_Read([DataSourceRequest] DataSourceRequest request)
        {
            var contacts = _TMDbContext.Contacts.Select(c => new Contact
            {
                ContactId = c.ContactId,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Message = c.Message,
                DateCreated = c.DateCreated
            }).OrderByDescending(c => c.ContactId);

            return Json(contacts.ToList().ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (model.Email.Contains("aol.com"))
                //{
                //    ModelState.AddModelError("", "We don't support AOL addresses");
                //}

                string body = this.createEmailBody(model);
                await _emailSender.SendEmailAsync("", "from Contact page", body);
                ModelState.Clear(); // Clear the form
                ViewBag.UserMessage = string.Format("Dear {0},{1}We appreciate you contacting us.{1} One of our colleagues will get back to you shortly.", model.Name, "\n"); //Notify Users

                //Insert Contact Information
                var newContact = Mapper.Map<Contact>(model);

                newContact.CreatedBy = model.Name;
                newContact.DateCreated = DateTime.UtcNow;

                newContact.UpdatedBy = model.Name;
                newContact.DateUpdated = DateTime.UtcNow;

                _contactRepo.Add(newContact);
                _contactRepo.SaveAll();
            }

            return View();
        }


        public IActionResult GetCustomerCoupons([DataSourceRequest] DataSourceRequest request)
        {
            var customerCoupons = _customerCouponRepo.GetAllNonExpired().ToList();
            return Json(customerCoupons.ToDataSourceResult(request));
        }

        public IActionResult Error()
        {
            return View();
        }

        private string createEmailBody(ContactViewModel model)
        {

            string body = string.Empty;

            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "ContactUs.html";

            body = System.IO.File.ReadAllText(pathToFile);

            body = body.Replace("{Name}", model.Name); //replacing the required things  
            body = body.Replace("{Phone}", model.Phone);
            body = body.Replace("{Email}", model.Email);
            body = body.Replace("{Message}", model.Message);
            return body;

        }
    }
}
