using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.Services
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class SelectedTabFilterAttribute:ActionFilterAttribute
    {
        private string _selectedTabFilter;

        public SelectedTabFilterAttribute(string selectedTabFilter)
        {
            _selectedTabFilter = selectedTabFilter;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = (Controller)context.Controller;
            controller.ViewBag.SelectedTabFilter = _selectedTabFilter;
        }
    }
}
