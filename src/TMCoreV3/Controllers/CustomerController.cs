using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.ViewModels.CustomerViewModels;
using TMCoreV3.DataAccess.Repos;

namespace TMCoreV3.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private CustomerRepository _CustomerRepository;

        public CustomerController(CustomerRepository CustomerRepository)
        {
            _CustomerRepository = CustomerRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
