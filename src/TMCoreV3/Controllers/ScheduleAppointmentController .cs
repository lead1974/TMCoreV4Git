using System;
using System.Collections.Generic;
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
using TMCoreV3.ViewModels.CustomerViewModels;
using TMCoreV3.DataAccess.Models.Customer;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TMCoreV3.Controllers
{
    public class ScheduleAppointmentController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private ICustomerApplianceTypeRepository _customerApplianceTypeRepo;
        private ICustomerApplianceBrandRepository _customerApplianceBrandRepo;
        private ICustomerRepository _customerRepo;

        public ScheduleAppointmentController(
            ICustomerRepository customerRepo,
            ICustomerApplianceTypeRepository customerApplianceTypeRepo,
            ICustomerApplianceBrandRepository customerApplianceBrandRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            ILoggerFactory loggerFactory)
        {
            _customerRepo = customerRepo;
            _customerApplianceBrandRepo = customerApplianceBrandRepo;
            _customerApplianceTypeRepo = customerApplianceTypeRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ScheduleAppointmentController>();
        }

        public IActionResult Index()
        {
            ViewBag.ScheduleConfirmed = "NO";
            ViewBag.SelectiveTab = "scheduleappointment";
            return View();
        }

        [HttpPost]
        public IActionResult Index(ScheduleAppointment form, string returnUrl)
        {
            ViewBag.SelectiveTab = "scheduleappointment";
            if (ModelState.IsValid)
            {
                var userName = form.Email;
                if (User.Identity.IsAuthenticated) userName = User.Identity.Name;

                var newSchedule = new Customer
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Address = form.Address,
                    City = form.City,
                    PhoneNumber = form.PhoneNumber,
                    PostalCode = form.PostalCode,
                    State = form.State,
                    Email = form.Email,                    
                    CreatedBy = userName,
                    UpdatedBy = userName,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    CustomerApplianceProblems = new List<CustomerApplianceProblem>()
                    {
                        new CustomerApplianceProblem() {
                            CustomerApplianceTypeId=form.CustomerApplianceTypeId,
                            CustomerApplianceBrandId=form.CustomerApplianceBrandId,                            
                            Problem=form.Problem,
                            DesiredScheduleTime=form.DesiredScheduleTime,
                            ModelNumber=form.ModelNumber,
                            ModelSerial=form.ModelSerial,
                            CreatedBy=userName,
                            DateCreated=DateTime.UtcNow,                            
                            ProblemStatus="NEW"
                        }
                     }                    
                    
                  };
                //Update and Save Here
                //_customerRepo.Add(newSchedule);
                //_customerRepo.SaveAll();

                //Send Email
                ViewBag.ScheduleConfirmed = "YES";
                return Confirmation(form);
            }                          

            return View(form);
        }

        [HttpGet]
        public ActionResult Confirmation(ScheduleAppointment newSchedule)
        {
           return View();
        }

        [HttpGet, Route("GetCustomerApplianceType")]
        public IActionResult GetCustomerApplianceTypes()
        {
            var customerApplianceTypes = _customerApplianceTypeRepo.GetAll().ToList();
            return Json(customerApplianceTypes);
        }

        [HttpGet, Route("GetCustomerApplianceBrand")]
        public IActionResult GetCustomerApplianceBrands(int? customerApplianceTypes)
        {
            var customerApplianceBrands = _customerApplianceBrandRepo.GetAll().AsQueryable();
            if (customerApplianceTypes!=null)
            {
                customerApplianceBrands = customerApplianceBrands.Where(p =>  p.CustomerApplianceTypeId == customerApplianceTypes);
            }
            return Json(customerApplianceBrands);
        }

        [HttpGet, Route("GetStates")]
        public IActionResult GetStates()
        {
            var states = new[]
            {
                new State
                {
                    StateId=1,
                    StateName="CA"
                },
                new State
                {
                    StateId = 2,
                    StateName = "NV"
                }
            };
            return Json(states);
        }
    }
}
