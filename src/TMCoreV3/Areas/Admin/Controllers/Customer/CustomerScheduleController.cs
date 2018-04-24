using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.DataAccess.Repos;
using TMCoreV3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using AutoMapper;

namespace TMCoreV3.Areas.Admin.Controllers.Customer
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite)]
    public class CustomerScheduleController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private ICustomerRepository _customerRepo;

        private const int PostsPerPage = 5;
        private GlobalService _globalService;

        public CustomerScheduleController(
            ICustomerRepository customerRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            GlobalService globalService,
            ILoggerFactory loggerFactory)
        {
            _customerRepo = customerRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _globalService = globalService;
            _logger = loggerFactory.CreateLogger <CustomerController>();
        }

        #region CustomerSchedule
        [HttpGet]
        public IActionResult CustomerScheduleIndex()
        {
            return View();
        }
        public IActionResult ScheduleInline_Read([DataSourceRequest] DataSourceRequest request)
        {
            var customerSchedules = _customerRepo.GetAllWithApplianceProblems().ToList();
            return Json(customerSchedules.ToDataSourceResult(request));
        }

        [HttpPost, Route("ScheduleInline_Update")]
        public IActionResult ScheduleInline_Update([DataSourceRequest] DataSourceRequest request, TMCoreV3.ViewModels.CustomerViewModels.ScheduleAppointment theSchedule)
        {
            if (theSchedule != null && ModelState.IsValid)
            {
                var customer = Mapper.Map<TMCoreV3.DataAccess.Models.Customer.Customer>(theSchedule);

                customer.UpdatedBy = User.Identity.Name;
                customer.DateUpdated = DateTime.UtcNow;

                _customerRepo.Update(customer);
                _customerRepo.SaveAll();
                return Json(new[] { customer }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        [HttpPost, Route("ScheduleInline_Destroy")]
        public IActionResult ScheduleInline_Destroy([DataSourceRequest] DataSourceRequest request, TMCoreV3.ViewModels.CustomerViewModels.ScheduleAppointment theSchedule)
        {
            if (theSchedule != null)
            {
                var customer = Mapper.Map<TMCoreV3.DataAccess.Models.Customer.Customer>(theSchedule);

                _customerRepo.Remove(customer);
                _customerRepo.SaveAll();
                return Json(new[] { customer }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        #endregion //CustomerSchedule
    }
}
