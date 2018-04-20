using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TMCoreV3.DataAccess;
using TMCoreV3.DataAccess.Models.User;
using Microsoft.Extensions.Logging;
using TMCoreV3.Areas.Admin.ViewModels.User;
using TMCoreV3.Services;

namespace TMCoreV3.api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly ILogger _logger;

        private TMDbContext _dmDbContext;        

        public UserController(
            TMDbContext dmContext,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _dmDbContext = dmContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<UserController>();
        }

        // GET: api/values
        [HttpGet]
        public async Task<IList<UserJquery>> Get()
        {
            var authUsers = _dmDbContext.Users;
            var users = authUsers.Select(u => new UserJquery
            {
                Id = u.Id,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed
            }).ToList();

            //Populate Roles for User
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
                user.Roles = string.Join(", ", userRoles);
            }

            return users;
        }

        private async Task<string> GetUserRoles(AuthUser u)
        {
            return string.Join(", ", await _userManager.GetRolesAsync(u));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public AuthUser Get(string id)
        {
            return _dmDbContext.Users.Where(u => u.Id==id).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(string email, [FromBody]AuthUser authUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return null;

                authUser.Email = user.Email;
                authUser.UserName = user.UserName;
                authUser.EmailConfirmed = user.EmailConfirmed;

                await _userManager.CreateAsync(authUser);
                return Created($"/api/user/{authUser.Email}", authUser.Email);
            }

            return BadRequest(ModelState);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("User to delete not found!");

                await _userManager.DeleteAsync(user);
                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
