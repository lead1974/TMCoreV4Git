using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TMCoreV3.Areas.Admin.ViewModels.Customer
{
    public class CustomerViewModel
    {

        public class CustomerIndex
        {
            public PageData<TMCoreV3.DataAccess.Models.Customer.Customer> Customers { get; set; }
        }

        public class CustomerForm
        {
            public int Id { get; set; }
            public bool IsNew { get; set; }

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

            [Display(Name = "Addess")]
            [Required]
            [DataType(DataType.Text)]
            public string Addess { get; set; }

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
            public string UpdatedBy { get; set; }
            public ICollection<CustomerApplianceProblem> CustomerApplianceProblems { get; set; }
        }

    }
}
