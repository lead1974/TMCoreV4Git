using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Models.User
{
    public class AuthRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
