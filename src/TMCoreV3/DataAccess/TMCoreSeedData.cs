using TMCoreV3.DataAccess.Models.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.DataAccess.Repos;
using TMCoreV3.Services;

namespace TMCoreV3.DataAccess
{
    public class TMCoreSeedData
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private TMDbContext _tmContext;
        private readonly ILogger _logger;

        private ICustomerRepository _customerRepo;
        private GlobalService _globalService;

        public TMCoreSeedData(
            TMDbContext tmContext,
            ICustomerRepository customerRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            GlobalService globalService,
            ILoggerFactory loggerFactory)
        {
            _tmContext = tmContext;
            _customerRepo = customerRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _globalService = globalService;
            _logger = loggerFactory.CreateLogger<TMCoreSeedData>();
        }

        public async Task EnsureSeedData()
        {
            await SeedAdminUsers();
            await SeedCustomerAndAppliances();
        }
        private async Task SeedAdminUsers()
        {
            var user = new AuthUser
            {
                UserName = "balda@balda.com",
                NormalizedUserName = "balda@balda.com",
                Email = "balda@balda.com",
                NormalizedEmail = "balda@balda.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<AuthRole>(_tmContext);

            if (!_tmContext.Roles.Any(r => r.Name == RoleName.CanManageSite))
            {
                await roleStore.CreateAsync(new AuthRole { Name = RoleName.CanManageSite, NormalizedName = RoleName.CanManageSite, Description="Site Administrator" });
            }

            if (!_tmContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AuthUser>();
                var hashed = password.HashPassword(user, "balda1234");
                user.PasswordHash = hashed;
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, RoleName.CanManageSite);
            }

            await _tmContext.SaveChangesAsync();
        }

        private async Task SeedCustomerAndAppliances()
        {
            if (!_tmContext.CustomerApplianceTypes.Any())
            {
                var customerApplianceType1 = new CustomerApplianceType()
                {
                    Name = "Air Conditioner",
                    Sequence = 1,
                    CreatedBy = "SYSTEM",
                    DateCreated = DateTime.UtcNow,
                    CustomerApplianceBrands = new List<CustomerApplianceBrand>()
                     {
                         new CustomerApplianceBrand() { Name= "American Standard", Sequence=1,CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Carrier", Sequence=2, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Crosley", Sequence=3, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Danby", Sequence=4, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Delonghi", Sequence=5, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Modern-Aire", Sequence=6, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Sterling", Sequence=7, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Tappan", Sequence=8, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Other", Sequence=999, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow }
                     }
                };
                _tmContext.CustomerApplianceTypes.Add(customerApplianceType1);
                _tmContext.CustomerApplianceBrands.AddRange(customerApplianceType1.CustomerApplianceBrands);

                var customerApplianceType2 = new CustomerApplianceType()
                {
                    Name = "BBQ",
                    Sequence = 2,
                    CreatedBy = "SYSTEM",
                    DateCreated = DateTime.UtcNow,
                    CustomerApplianceBrands = new List<CustomerApplianceBrand>()
                     {
                         new CustomerApplianceBrand() { Name= "Bertazzoni", Sequence=1,CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Bosh", Sequence=2, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Brown", Sequence=3, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Chambers", Sequence=4, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Dacor", Sequence=5, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Danby", Sequence=6, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Delonghi", Sequence=7, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Dynasty", Sequence=8, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Dynasty", Sequence=9, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Frigidaire", Sequence=10, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "KitchenAid", Sequence=11, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Samsung", Sequence=12, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Viking", Sequence=13, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Wold", Sequence=14, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Zephyr", Sequence=15, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Other", Sequence=999, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow }
                     }
                };
                _tmContext.CustomerApplianceTypes.Add(customerApplianceType2);
                _tmContext.CustomerApplianceBrands.AddRange(customerApplianceType2.CustomerApplianceBrands);

                var customerApplianceType3 = new CustomerApplianceType()
                {
                    Name = "Cooktop",
                    Sequence = 3,
                    CreatedBy = "SYSTEM",
                    DateCreated = DateTime.UtcNow,
                    CustomerApplianceBrands = new List<CustomerApplianceBrand>()
                     {
                         new CustomerApplianceBrand() { Name= "Avanti", Sequence=1, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Bertazzoni", Sequence=2, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Bosch", Sequence=3, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Brown", Sequence=4, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Caloric", Sequence=5, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Chambers", Sequence=6, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Dacor", Sequence=7, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "DCS", Sequence=8, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Delonghi", Sequence=9, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow},
                         new CustomerApplianceBrand() { Name= "Dynasty", Sequence=10, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Electrolux", Sequence=11, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Frigidaire", Sequence=12, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "KitchenAid", Sequence=13, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Samsung", Sequence=14, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Viking", Sequence=15, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Wold", Sequence=16, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Zephyr", Sequence=17, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow },
                         new CustomerApplianceBrand() { Name= "Other", Sequence=999, CreatedBy="SYSTEM", DateCreated=DateTime.UtcNow }
                     }
                };
                _tmContext.CustomerApplianceTypes.Add(customerApplianceType3);
                _tmContext.CustomerApplianceBrands.AddRange(customerApplianceType3.CustomerApplianceBrands);
                await _tmContext.SaveChangesAsync();
            }

            if (!_tmContext.Customers.Any())
            {
                var tcustomer = new Customer()
                {
                    DateCreated = DateTime.UtcNow,
                    FirstName = "Timur",
                    LastName = "Alayev",
                    Email = "tim@yahoo.com",
                    PhoneNumber = "7145293374",
                    CreatedBy = "tim@yahoo.com",
                    City = "La Habra",
                    PostalCode = "92788",
                    State = "CA",
                    Address = "1241 Robin Way",

                    CustomerApplianceProblems = new List<CustomerApplianceProblem>()
                    {
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=1,
                            CustomerApplianceBrandId=1,
                            DateCreated=DateTime.UtcNow,
                            Problem="Gas leaking",
                            ModelNumber="123123123SDfR",
                            ModelSerial="sdfsldkjfSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="NEW"
                        },
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=2,
                            CustomerApplianceBrandId=3,
                            DateCreated=DateTime.UtcNow,
                            Problem="Water leaking",
                            ModelNumber="2344422333123SDfR",
                            ModelSerial="aaasddffSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="NEW"
                        },
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=3,
                            CustomerApplianceBrandId=2,
                            DateCreated=DateTime.UtcNow,
                            Problem="Stop working",
                            ModelNumber="12asd3123SDfR",
                            ModelSerial="ddddddaaakjfSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="NEW"
                        }
                    }
                };

                _tmContext.Customers.Add(tcustomer);
                _tmContext.CustomerApplianceProblems.AddRange(tcustomer.CustomerApplianceProblems);

                var acustomer = new Customer()
                {
                    DateCreated = DateTime.UtcNow,
                    FirstName = "Andrey",
                    LastName = "GoodEvening",
                    Email = "andrey@gmail.com",
                    PhoneNumber = "7143453374",
                    CreatedBy = "andrey@gmail.com",
                    City = "Garden Grove",
                    PostalCode = "93744",
                    State = "CA",
                    Address = "14455 Yankee Way",

                    CustomerApplianceProblems = new List<CustomerApplianceProblem>()
                    {
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=1,
                            CustomerApplianceBrandId=1,
                            DateCreated=DateTime.UtcNow,
                            Problem="Terrible noise",
                            ModelNumber="123123123SDfR",
                            ModelSerial="sdfsldkjfSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="APPROVED"
                        },
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=2,
                            CustomerApplianceBrandId=3,
                            DateCreated=DateTime.UtcNow,
                            Problem="No idea, need you to check",
                            ModelNumber="2344422333123SDfR",
                            ModelSerial="aaasddffSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="APPROVED"
                        },
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=3,
                            CustomerApplianceBrandId=2,
                            DateCreated=DateTime.UtcNow,
                            Problem="Need to replace filter",
                            ModelNumber="12asd3123SDfR",
                            ModelSerial="ddddddaaakjfSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="CANCELLED"
                        }
                    }
                };

                _tmContext.Customers.Add(acustomer);
                _tmContext.CustomerApplianceProblems.AddRange(acustomer.CustomerApplianceProblems);

                var bcustomer = new Customer()
                {
                    DateCreated = DateTime.UtcNow,
                    FirstName = "Michael",
                    LastName = "Smith",
                    Email = "msmith@yahoo.com",
                    PhoneNumber = "7145223374",
                    CreatedBy = "msmith@gmail.com",
                    City = "Fullerton",
                    PostalCode = "92783",
                    State = "CA",
                    Address = "2344 Russki Way",

                    CustomerApplianceProblems = new List<CustomerApplianceProblem>()
                    {
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=1,
                            CustomerApplianceBrandId=1,
                            DateCreated=DateTime.UtcNow,
                            Problem="Terrible noise",
                            ModelNumber="asdg23SDfR",
                            ModelSerial="AAAAAfSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(1),
                            ProblemStatus="NEW"
                        },
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=2,
                            CustomerApplianceBrandId=3,
                            DateCreated=DateTime.UtcNow,
                            Problem="Water leaking",
                            ModelNumber="RRRRRRR123SDfR",
                            ModelSerial="EEEEEEEdffSDF",
                            CreatedBy="SYSTEM",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(3),
                            ProblemStatus="NEW"
                        },
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=3,
                            CustomerApplianceBrandId=2,
                            DateCreated=DateTime.UtcNow,
                            Problem="Shaking",
                            ModelNumber="EERRTTYYYY3SDfR",
                            ModelSerial="WERRTYakjfSDF",
                            DesiredScheduleTime=DateTime.UtcNow.AddDays(2),
                            ProblemStatus="NEW"
                        }
                    }
                };

                _tmContext.Customers.Add(bcustomer);
                _tmContext.CustomerApplianceProblems.AddRange(bcustomer.CustomerApplianceProblems);

                var customerCoupons = new List<CustomerCoupon>()
                    {
                        new CustomerCoupon() {
                            Name="20% OFF LABOR",
                            Description = "On repair, service and installation.Please visit our site for new coupons.",
                            ExpirationDate=DateTime.UtcNow.AddDays(30),
                            Sequence = 1
                        },
                        new CustomerCoupon() {
                            Name="SENIOR DISCOUNT 25%",
                            Description = "On repair, service and installation.Please visit our site for new coupons.",
                            ExpirationDate=DateTime.UtcNow.AddDays(120),
                            Sequence = 2
                        },
                         new CustomerCoupon() {
                            Name="20% OFF VISIT",
                            Description = "On repair, service and installation.Please visit our site for new coupons.",
                            ExpirationDate=DateTime.UtcNow.AddDays(30),
                            Sequence = 3
                        },
                        new CustomerCoupon() {
                            Name="25% DISCOUNT FOR PARTS",
                            Description = "On repair, service and installation.Please visit our site for new coupons.",
                            ExpirationDate=DateTime.UtcNow.AddDays(120),
                            Sequence = 4
                        }
                    };

                _tmContext.CustomerCoupons.AddRange(customerCoupons);

                await _tmContext.SaveChangesAsync();

            }
        }
    }
}
