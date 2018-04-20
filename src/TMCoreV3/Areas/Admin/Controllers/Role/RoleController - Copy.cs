using TMCoreV3.Areas.Admin.ViewModels.Role;
using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Models.User;
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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using AutoMapper;

namespace TMCoreV3.Areas.Admin.Controllers.Role
{
    [Area("Admin")]    
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite)]
    public class Role2Controller:Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private TMDbContext _TMDbContext;

        public Role2Controller(
            TMDbContext tmContext,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            ILoggerFactory loggerFactory)
        {
            _TMDbContext = tmContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<RoleController>();
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult EditingPopup_Read([DataSourceRequest] DataSourceRequest request)
        {
            var roles = _TMDbContext.Roles.Select(r => new RoleEdit
            {
                 Id = r.Id,
                 Name = r.Name,
                 Description = r.Description
            });

            return Json(roles.ToList().ToDataSourceResult(request));
        }

        //[HttpPost]
        //public IActionResult EditingPopup_Create([DataSourceRequest] DataSourceRequest request, RoleEdit theRole)
        //{
        //    var newRole = Mapper.Map<AuthRole>(theRole);
        //    if (theRole != null && ModelState.IsValid)
        //    {
        //        if (!_roleManager.RoleExistsAsync(theRole.Name).Result)
        //        {
        //            IdentityResult roleResult = _roleManager.CreateAsync(newRole).Result;
        //            if (!roleResult.Succeeded)
        //            {
        //                ModelState.AddModelError("", "Error while creating role: " + roleResult.Errors.ToList());
        //            }
        //        }
        //        return Json(new[] { newRole }.ToDataSourceResult(request, ModelState));
        //    }

        //    return BadRequest();
        //}

        [HttpPost]
        public async Task<ActionResult> EditingPopup_Update([DataSourceRequest] DataSourceRequest request, RoleEdit theRole)
        {
            if (theRole != null && ModelState.IsValid)
            {
                var newRole = Mapper.Map<AuthRole>(theRole);

                var role = await _roleManager.FindByIdAsync(newRole.Id);
                if (role == null) return null;
                await _roleManager.UpdateAsync(newRole);

                return Json(new[] { newRole }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        //[HttpGet, Route("newrole")]
        //public IActionResult NewRole()
        //{
        //    return View(new RoleNew
        //    {
        //    });
        //}

        //[HttpPost, Route("newrole")]
        //public IActionResult NewRole(RoleNew form, string returnUrl, string action)
        //{
        //    if (action == "Create")
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (!_roleManager.RoleExistsAsync(form.Name).Result)
        //            {
        //                var role = new AuthRole();
        //                role.Name = form.Name;
        //                role.Description = form.Description;

        //                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
        //                if (!roleResult.Succeeded)
        //                {
        //                    ModelState.AddModelError("", "Error while creating role: " + roleResult.Errors.ToList());
        //                    return View(form);
        //                }

        //                if (string.IsNullOrWhiteSpace(returnUrl))
        //                {
        //                    return RedirectToAction("index", "role");
        //                }
        //                else { return Redirect(returnUrl); }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Role creation failed: Role already exist!");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("index", "role");              
        //    }

        //    return View();
        //}

        //[HttpGet, Route("editrole")]
        //public async Task<IActionResult> EditRole(string Id)
        //{
        //    RoleEdit roleEdit = await GetEditRoles(Id);

        //    return View(roleEdit);
        //}

        //private async Task<RoleEdit> GetEditRoles(string Id)
        //{
        //    var role = await _roleManager.FindByIdAsync(Id);
        //    if (role == null) RedirectToAction("index", "role");

        //    var roleEdit = new RoleEdit
        //    {
        //        Id = Id,
        //        Name = role.Name,
        //        Description = role.Description
        //    };
        //    return roleEdit;
        //}

        //[HttpPost, Route("editrole")]
        //public async Task<IActionResult> EditRole(RoleEdit form,  string returnUrl, string action)
        //{
        //    if (action == "Edit")
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var role = await _roleManager.FindByIdAsync(form.Id);
        //            var roleByName = await _roleManager.FindByNameAsync(form.Name);
        //            if (roleByName != null && roleByName.Id != form.Id)
        //            {
        //                ModelState.AddModelError("", "Role Name update failed: role with the same role name already exist!");
        //                return View();
        //            }

        //            if (role != null)
        //            {
        //                role.Name = form.Name;
        //                role.Description = form.Description;
        //                await _roleManager.UpdateAsync(role);

        //                if (string.IsNullOrWhiteSpace(returnUrl))
        //                {
        //                    return RedirectToAction("Index", "Role");
        //                }
        //                else { return Redirect(returnUrl); }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "Role update failed: role not found!");
        //            }
        //        }             
        //    }
        //    else
        //    {
        //        return RedirectToAction("index", "role");
        //    }

        //    RoleEdit roleEdit = await GetEditRoles(form.Id);

        //    return View(roleEdit);
        //}

        //[HttpGet, Route("deleterole")]
        //public async Task<IActionResult> DeleteRole(string Id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var role = await _roleManager.FindByIdAsync(Id);
        //        if (role == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Role to delete not found!");
        //        }
        //        await _roleManager.DeleteAsync(role);
        //    }
        //    return RedirectToAction("Index", "Role");
        //}
    }
}
