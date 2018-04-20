using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.DataAccess.Repos;
using TMCoreV3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TMCoreV3.Areas.Admin.ViewModels.Customer.CustomerViewModel;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TMCoreV3.DataAccess.Models.Customer;
using AutoMapper;

namespace TMCoreV3.Areas.Admin.Controllers.Customer
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite)]
    public class CustomerCouponController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private ICustomerRepository _customerRepo;
        private ICustomerCouponRepository _customerCouponRepo;

        private const int PostsPerPage = 5;
        private GlobalService _globalService;

        public CustomerCouponController(
            ICustomerRepository customerRepo,
            ICustomerCouponRepository customerCouponRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            GlobalService globalService,
            ILoggerFactory loggerFactory)
        {
            _customerRepo = customerRepo;
            _customerCouponRepo = customerCouponRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _globalService = globalService;
            _logger = loggerFactory.CreateLogger <CustomerController>();
        }

        #region CustomerCouponCoupon
        [HttpGet]
        public IActionResult CouponIndex()
        {
            return View();
        }
        public IActionResult CouponInline_Read([DataSourceRequest] DataSourceRequest request)
        {
            var customerCoupons = _customerCouponRepo.GetAll().ToList();
            return Json(customerCoupons.ToDataSourceResult(request));
        }

        [HttpPost, Route("CouponInline_Create")]
        public IActionResult CouponInline_Create([DataSourceRequest] DataSourceRequest request, CustomerCoupon theCoupon)
        {
            if (theCoupon != null && ModelState.IsValid)
            {
                var newCoupon = Mapper.Map<DataAccess.Models.Customer.CustomerCoupon>(theCoupon);

                newCoupon.CreatedBy = User.Identity.Name;
                newCoupon.DateCreated = DateTime.UtcNow;

                newCoupon.UpdatedBy = User.Identity.Name;
                newCoupon.DateUpdated = DateTime.UtcNow;

                _customerCouponRepo.Add(newCoupon);
                _customerCouponRepo.SaveAll();
                return Json(new[] { newCoupon }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        [HttpPost, Route("CouponInline_Update")]
        public IActionResult CouponInline_Update([DataSourceRequest] DataSourceRequest request, CustomerCoupon theCoupon)
        {
            if (theCoupon != null && ModelState.IsValid)
            {
                var newCoupon = Mapper.Map<DataAccess.Models.Customer.CustomerCoupon>(theCoupon);

                newCoupon.CreatedBy = User.Identity.Name;
                newCoupon.DateCreated = DateTime.UtcNow;

                newCoupon.UpdatedBy = User.Identity.Name;
                newCoupon.DateUpdated = DateTime.UtcNow;

                _customerCouponRepo.Update(newCoupon);
                _customerCouponRepo.SaveAll();
                return Json(new[] { newCoupon }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        [HttpPost, Route("CouponInline_Destroy")]
        public IActionResult CouponInline_Destroy([DataSourceRequest] DataSourceRequest request, CustomerCoupon theCoupon)
        {
            if (theCoupon != null)
            {
                var newCoupon = Mapper.Map<DataAccess.Models.Customer.CustomerCoupon>(theCoupon);

                _customerCouponRepo.Delete(newCoupon);
                _customerCouponRepo.SaveAll();
                return Json(new[] { newCoupon }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        #endregion //CustomerCoupon
    }
}
