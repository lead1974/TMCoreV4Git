using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMWork.DataAccess.Models.Customer
{
    public class Contact
    {
        [Key]
        [Required]
        public int ContactId { get; set; }

        [Display(Name = "Name")]
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Phone")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Message")]
        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
