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

namespace TMCoreV3.Areas.Admin.Controllers.Customer
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageCustomers)]
    public class CustomerController : Controller
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

        public CustomerController(
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

        #region Customer
        [HttpGet, Route("customerIndex")]
        public IActionResult CustomerIndex(int page = 1)
        {
            var totalPostCount = _customerRepo.GetAll().ToList().Count();
            var currentCustomerPage = _customerRepo.GetAll()
                .OrderByDescending(c => c.DateCreated)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();

            return View(new CustomerIndex
            {
                Customers = new PageData<TMCoreV3.DataAccess.Models.Customer.Customer>(currentCustomerPage, totalPostCount, page, PostsPerPage)
            });
        }

        [HttpGet, Route("customerform")]
        public IActionResult EditPost(string returnUrl, string action)
        {
            var post = new CustomerForm
            {
                IsNew = true
            };
            return View("CustomerForm", post);
        }

        [HttpPost, Route("customerform"), ValidateAntiForgeryToken]
        public IActionResult Form(CustomerForm form, string returnUrl, string action)
        {
            if (action == "CreatePost" || action == "UpdatePost")
            {
                form.IsNew = _globalService.IsNullOrDefault(form.Id);
                if (!ModelState.IsValid) return View(form);

                TMCoreV3.DataAccess.Models.Customer.Customer customer;
                if (form.IsNew)
                {
                    customer = new TMCoreV3.DataAccess.Models.Customer.Customer
                    {
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = User.Identity.Name,
                    };
                }
                else
                {
                    customer = _customerRepo.FindById(form.Id);
                    if (customer == null)
                    {
                        ModelState.AddModelError("", "Customer update failed: Customer not found");
                    }

                    customer.DateUpdated = DateTime.UtcNow;
                    customer.UpdatedBy = User.Identity.Name;

                }

                _customerRepo.Add(customer);
                _customerRepo.SaveAll();
            }
            else if (action == "Cancel")
                return RedirectToAction("customerIndex", "customer");

            return RedirectToAction("customerIndex", "customer");
        }
        #endregion Customer

        #region CustomerCoupon
        [HttpGet, Route("couponIndex")]
        public IActionResult CouponIndex()
        {
            var customerCoupons = _customerCouponRepo.GetAll().ToList();
            return View(customerCoupons);
        }

        #endregion //CustomerCoupon
    }
}
