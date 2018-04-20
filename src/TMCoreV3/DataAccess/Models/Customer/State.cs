using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Models.Customer
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string StateName { get; set; }
    }
}
