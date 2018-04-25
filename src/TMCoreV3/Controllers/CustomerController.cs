using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TMWork.DataAccess.Models.Customer;
using TMWork.ViewModels.CustomerViewModels;
using TMWork.DataAccess.Repos;

namespace TMWork.Controllers
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
