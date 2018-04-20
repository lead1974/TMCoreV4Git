using TMCoreV3.DataAccess.Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Models.Customer
{
    public class Customer
    {
        [Key]
        [Required]
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "First Name must be between 2 and 100 characters long")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 2,
           ErrorMessage = "Last Name must be between 2 and 100 characters long")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [Required]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        [DataType(DataType.Text)]
        public string State { get; set; }

        [Display(Name = "Postal Code")]
        [Required]
        [DataType(DataType.Text)]
        public string PostalCode { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<CustomerApplianceProblem> CustomerApplianceProblems { get; set; }

    }
}
