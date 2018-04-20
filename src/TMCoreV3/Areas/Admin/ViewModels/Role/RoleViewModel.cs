using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.Areas.Admin.ViewModels.Role
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {

        }

    }

    public class RoleIndex
    {
        public IEnumerable<AuthRole> Roles { get; set; }
    }

    public class RoleNew
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }

    public class RoleEdit
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }

}
