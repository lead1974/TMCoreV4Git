using TMCoreV3.DataAccess.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.Areas.Admin.ViewModels.User
{
    public class UserViewModel
    {
    }

    public class RoleCheckBox
    {
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }
    public class UserJquery
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Roles { get; set; }
    }
    public class UserIndexJquery
    {
        public IEnumerable<UserJquery> Users { get; set; }
    }
    public class UserIndex
    {
        public IEnumerable<AuthUser> Users { get; set; }
    }

    public class UserNew
    {
        public IList<RoleCheckBox> CheckBoxRoles { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Email Address must be between 4 and 256 characters long")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }
    }

    public class UserEdit
    {
        public IList<RoleCheckBox> CheckBoxRoles { get; set; }
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed?")]
        public bool EmailConfirmed { get; set; }
    }

    public class UserResetPassword
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
