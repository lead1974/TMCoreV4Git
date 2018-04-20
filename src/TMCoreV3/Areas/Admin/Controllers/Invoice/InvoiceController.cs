using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TMCoreV3.Services;
using Microsoft.Extensions.Logging;
using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.DataAccess.Repos;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCoreV3.Areas.Admin.Controllers.Invoice
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite)]
    public class InvoiceController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private IInvoiceRepository _invoiceRepo;

        private const int PostsPerPage = 5;
        private GlobalService _globalService;

        public InvoiceController(
            IInvoiceRepository invoiceRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            GlobalService globalService,
            ILoggerFactory loggerFactory)
        {
            _invoiceRepo = invoiceRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _globalService = globalService;
            _logger = loggerFactory.CreateLogger<InvoiceController>();
        }
        // GET: /<controller>/
        public IActionResult InvoiceIndex()
        {
            return View();
        }

        [HttpGet, Route("NewInvoice")]
        public IActionResult NewInvoice()
        {
            return View();
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
