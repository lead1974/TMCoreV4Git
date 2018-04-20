using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TMCoreV3.DataAccess.Repos;
using TMCoreV3.DataAccess;
using TMCoreV3.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TMCoreV3.ViewModels.CustomerViewModels;
using AutoMapper;
using System.Net;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCoreV3.api.Customer
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private TMDbContext _tmContext;
        private ICustomerRepository _customerRepo;
        private GlobalService _globalService;

        public CustomerController(
            TMDbContext tmContext,
            ICustomerRepository customerRepo,
            GlobalService globalService)
        {
            _tmContext = tmContext;
            _customerRepo = customerRepo;
            _globalService = globalService;
        }

        // GET: api/values
        [HttpGet,Route("read")]
        public DataSourceResult GetCustomer([DataSourceRequest] DataSourceRequest request)
        {
            var customers = _customerRepo.GetAll().ToList().ToDataSourceResult(request);
            return customers;
        }

        // GET api/values/5
        [HttpGet("{id}"), Route("read")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _customerRepo.FindById(id);
            return Json(customer);
        }

        // POST api/values

        [HttpPost, Route("create")]
        public IActionResult PostCustomer([FromBody]CustomerForm theCustomer)
        {
            if (!ModelState.IsValid || theCustomer == null)
            {
                return BadRequest(ModelState);
            }
            var newCustomer = Mapper.Map<DataAccess.Models.Customer.Customer>(theCustomer);

            newCustomer.DateCreated = DateTime.UtcNow;
            newCustomer.CreatedBy = "SYSTEM";

            _customerRepo.Add(newCustomer);
            _customerRepo.SaveAll();
            return Created($"api/customer/{newCustomer.CustomerId}", newCustomer);
        }

        // PUT api/values/5
        [HttpPut("{id}"), Route("update")]
        public IActionResult PutCustomer([FromBody]CustomerForm theCustomer)
        {
            //string Input = JsonConvert.SerializeObject(theCustomer);

            if (!ModelState.IsValid || theCustomer == null)
            {
                return BadRequest(ModelState);
            }

            var newCustomer = Mapper.Map<DataAccess.Models.Customer.Customer>(theCustomer);
            newCustomer.DateUpdated = DateTime.UtcNow;
            newCustomer.UpdatedBy = "SYSTEM";
            _customerRepo.SaveAll();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(Mapper.Map<CustomerForm>(newCustomer));
        }

        [HttpDelete, Route("destroy")]
        public IActionResult DeleteCustomer([FromBody]CustomerForm theCustomer)
        {
            return DeleteCustomer(theCustomer.CustomerId);
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Route("destroy")]
        public IActionResult DeleteCustomer(int id)
        {
            DataAccess.Models.Customer.Customer theCustomer = _customerRepo.FindById(id);
            if (theCustomer == null)
            {
                return BadRequest(ModelState);
            }

            _customerRepo.Remove(theCustomer);
            _customerRepo.SaveAll();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(Mapper.Map<CustomerForm>(theCustomer));
        }
    }
}
