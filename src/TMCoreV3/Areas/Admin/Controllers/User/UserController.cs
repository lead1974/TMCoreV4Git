using TMCoreV3.Areas.Admin.ViewModels.User;
using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.ObjectModel;

namespace TMCoreV3.Areas.Admin.Controllers.User
{
    [Area("Admin")]    
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite)]
    public class UserController:Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private TMDbContext _TMDbContext;

        public UserController(
            TMDbContext dmContext,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            ILoggerFactory loggerFactory)
        {
            _TMDbContext = dmContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<UserController>();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            //List<AuthUser> authUsers = _TMDbContext.Users.Include(u => u.Roles).ToList();
            ////foreach (var user in authUsers)
            ////{
            ////    var Roles = _userManager.GetRolesAsync(user);
            ////}
            //return View(new UserIndex
            //{
            //    Users = authUsers
            //});

            return View();
        }

        public async Task<IActionResult> EditingPopup_Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = _TMDbContext.Users.Include(u => u.Roles).ToList();
            List<UserJquery> ujList = new List<UserJquery>();
            string str;
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                str = "";
                foreach (var role in userRoles)
                {
                    str = (str == "") ? role.ToString() : str + ", " + role.ToString();
                }

                var userJquery = new UserJquery
                {
                    Id = user.Id,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = str

                };

                ujList.Add(userJquery);

            }
          
            return Json(ujList.ToDataSourceResult(request));
        }

        [HttpGet, Route("newuser")]
        public IActionResult NewUser()
        {
            List<AuthRole> authRoles = _roleManager.Roles.ToList();
            return View(new UserNew
            {
                CheckBoxRoles = authRoles.Select(role => new RoleCheckBox
                {
                   Id = role.Id,
                   IsChecked = false,
                   Name = role.Name
                }).ToList()
            });
        }

        [HttpPost, Route("newuser")]
        public async Task<IActionResult> NewUser(UserNew form, string returnUrl, string action)
        {
            if (action == "Create")
            {
                if (ModelState.IsValid)
                {
                    if (await _userManager.FindByEmailAsync(form.Email) == null)
                    {
                        var user = new AuthUser();

                        user.UserName = form.Email;
                        user.Email = form.Email;
                        user.EmailConfirmed = form.EmailConfirmed;
                        user.NormalizedEmail = form.Email.ToUpper();

                        await _userManager.CreateAsync(user, form.Password);

                        foreach (var role in form.CheckBoxRoles)
                        {
                            if (role.IsChecked)
                                await _userManager.AddToRoleAsync(user, role.Name);
                            else
                                await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }

                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("Index", "User");
                        }
                        else { return Redirect(returnUrl); }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User creation failed: User's email must be unique!");
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("index","user");
            }

            //in case it failed to create new user and need to show Errors on the screen
            List<AuthRole> authRoles = _roleManager.Roles.ToList();
            return View(new UserNew
            {
                CheckBoxRoles = authRoles.Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList()
            });

        }

        [HttpGet, Route("edituser")]
        public async Task<IActionResult> EditUser(string Id)
        {
            UserEdit userEdit = await GetUserEdit(Id);

            return View(userEdit);
        }

        private async Task<UserEdit> GetUserEdit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) RedirectToAction("index", "user");

            var authRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var userEdit = new UserEdit
            {
                Id = Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                CheckBoxRoles = authRoles.Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = userRoles.Contains(role.Name),
                    Name = role.Name
                }).ToList()
            };
            return userEdit;
        }

        [HttpPost, Route("edituser")]
        public async Task<IActionResult> EditUser(UserEdit form,  string returnUrl, string action)
        {
            if (action == "Edit")
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(form.Id);
                    var userByEmail = await _userManager.FindByEmailAsync(form.Email);
                    if (userByEmail != null && userByEmail.Id != form.Id)
                    {
                        ModelState.AddModelError("", "User update failed: user with the same email already exist!");
                        return View();
                    }

                    if (ModelState.IsValid)
                    {
                        if (user != null)
                        {
                            user.UserName = form.Email;
                            user.Email = form.Email;
                            user.EmailConfirmed = form.EmailConfirmed;
                            await _userManager.UpdateAsync(user);

                            foreach (var role in form.CheckBoxRoles)
                            {
                                if (role.IsChecked)
                                    await _userManager.AddToRoleAsync(user, role.Name);
                                else
                                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                            }

                            if (string.IsNullOrWhiteSpace(returnUrl))
                            {
                                return RedirectToAction("Index", "User");
                            }
                            else { return Redirect(returnUrl); }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User update failed: user not found!");
                        }
                    }                    
                }
            }
            else
            {
                return RedirectToAction("index", "user");
            }

            UserEdit userEdit = await GetUserEdit(form.Id);

            return View(userEdit);
        }

        [HttpGet, Route("deleteuser")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User to delete not found!");
                }
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index", "User");
        }

        [HttpGet, Route("resetpassword")]
        public async Task<ActionResult> ResetPassword(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Index", "User");
            }

            return View(new UserResetPassword
            {
                Email = user.Email,
                Password=string.Empty,
                ConfirmPassword=string.Empty,
                Code=string.Empty
            });
        }

        [HttpPost, Route("resetpassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(UserResetPassword form, string action)
        {
            if (action == "Reset")
            {
                if (!ModelState.IsValid)
                {
                    return View(form);
                }
                var user = await _userManager.FindByEmailAsync(form.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    ModelState.AddModelError("", "User to reset password not found!");
                    return View();
                }

                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, form.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "user");
                }
                else
                {
                    ModelState.AddModelError("", "Reset Password failed: ");
                }
            }
            else
            {
                return RedirectToAction("index", "user");
            }
 
            return View();
        }

    }
}
