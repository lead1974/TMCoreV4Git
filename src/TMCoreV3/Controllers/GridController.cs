using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.ViewModels.CustomerViewModels;
using TMCoreV3.DataAccess.Repos;
using Kendo.Mvc.UI;
using TMCoreV3.DataAccess;
using TMCoreV3.Services;
using Kendo.Mvc.Extensions;
using AutoMapper;

namespace TMCoreV3.Controllers
{
    public partial class GridController : Controller
    {
        private TMDbContext _tmContext;
        private ICustomerRepository _customerRepo;
        private GlobalService _globalService;

        public GridController(
            TMDbContext tmContext,
            ICustomerRepository customerRepo,
            GlobalService globalService)
        {
            _tmContext = tmContext;
            _customerRepo = customerRepo;
            _globalService = globalService;
        }

        public IActionResult Editing_Popup()
        {
            return View();
        }

        public IActionResult EditingPopup_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_customerRepo.GetAll().ToList().ToDataSourceResult(request));
        }

        [HttpPost]
        public IActionResult EditingPopup_Create([DataSourceRequest] DataSourceRequest request, CustomerForm theCustomer)
        {
            if (theCustomer != null && ModelState.IsValid)
            {
                var newCustomer = Mapper.Map<DataAccess.Models.Customer.Customer>(theCustomer);

                newCustomer.DateCreated = DateTime.UtcNow;
                newCustomer.CreatedBy = "SYSTEM";
                newCustomer.Address = "123 Test";
                newCustomer.City = "Test";
                newCustomer.PhoneNumber = "23423424";
                newCustomer.PostalCode = "92344";
                newCustomer.State = "CA";
                
                _customerRepo.Add(newCustomer);
                _customerRepo.SaveAll();
                return Json(new[] { newCustomer }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        [HttpPost]
        public ActionResult EditingPopup_Update([DataSourceRequest] DataSourceRequest request, CustomerForm theCustomer)
        {
            if (theCustomer != null && ModelState.IsValid)
            {
                var newCustomer = Mapper.Map<DataAccess.Models.Customer.Customer>(theCustomer);
                newCustomer.DateUpdated = DateTime.UtcNow;
                newCustomer.UpdatedBy = "SYSTEM";
                newCustomer.Address = "123 Test";
                newCustomer.City = "Test";
                newCustomer.PhoneNumber = "23423424";
                newCustomer.PostalCode = "92344";
                newCustomer.State = "CA";

                _customerRepo.Update(newCustomer);
                _customerRepo.SaveAll();
                return Json(new[] { newCustomer }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        [HttpDelete]
        public ActionResult EditingPopup_Destroy([DataSourceRequest] DataSourceRequest request, CustomerForm theCustomer)
        {
            if (theCustomer != null)
            {
                var newCustomer = Mapper.Map<DataAccess.Models.Customer.Customer>(theCustomer);
                _customerRepo.Remove(newCustomer);
                _customerRepo.SaveAll();
                return Json(new[] { newCustomer }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

    }
}
