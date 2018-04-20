using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.Services;
using TMCoreV3.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TMCoreV3.ViewModels.CustomerViewModels
{
    public class CustomerIndex
    {
        public CustomerIndex()
        {

        }
        public IEnumerable<Customer> Customers { get; set; }
    }
    public class CustomerForm
    {
        public CustomerForm()
        {

        }

        [ScaffoldColumn(false)]
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

        //public string CustomerName {
        //    get { return string.Format("{0} {1}", FirstName, LastName); }
        //    set { string.Format("{0} {1}", FirstName, LastName); }
        //}

        //[Display(Name = "Phone Number")]
        //[Required]
        //[DataType(DataType.PhoneNumber)]
        //public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Display(Name = "Addess")]
        //[Required]
        //[DataType(DataType.Text)]
        //public string Addess { get; set; }

        //[Display(Name = "City")]
        //[Required]
        //[DataType(DataType.Text)]
        //public string City { get; set; }

        //[Display(Name = "State")]
        //[Required]
        //[DataType(DataType.Text)]
        //public string State { get; set; }

        //[Display(Name = "Postal Code")]
        //[Required]
        //[DataType(DataType.Text)]
        //public string PostalCode { get; set; }

        //public DateTime DateCreated { get; set; }
        //public DateTime? DateUpdated { get; set; }
        //public string CreatedBy { get; set; }
        //public string UpdatedBy { get; set; }
        //public ICollection<CustomerApplianceProblem> CustomerApplianceProblems { get; set; }
    }

    public class ScheduleAppointment
    {

        [ScaffoldColumn(false)]
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "First Name must be between 2 and 100 characters long")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 2,
           ErrorMessage = "Last Name must be between 2 and 100 characters long")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Display(Name = "State")]
        [DataType(DataType.Text)]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is required")]
        [DataType(DataType.Text)]
        public string PostalCode { get; set; }

        [Display(Name = "Appliance Type")]
        [Required(ErrorMessage = "Appliance Type is required")]
        [DataType(DataType.Text)]
        public int CustomerApplianceTypeId { get; set; }

        [Display(Name = "Appliance Brand")]
        [Required(ErrorMessage = "Appliance Brand is required")]
        [DataType(DataType.Text)]
        public int CustomerApplianceBrandId { get; set; }

        [Display(Name = "Model Number")]
        [DataType(DataType.Text)]
        public string ModelNumber { get; set; }

        [Display(Name = "Model Serial")]
        [DataType(DataType.Text)]
        public string ModelSerial { get; set; }

        [Display(Name = "Desired Schedule Time")]
        [Required(ErrorMessage = "Desired Date/Time Schedule is required")]
        [DataType(DataType.DateTime)]
        public DateTime DesiredScheduleTime { get; set; }

        [Display(Name = "Brief Problem Description")]
        [Required(ErrorMessage = "Brief Problem Description is required")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5,ErrorMessage = "Brief Problem Description must be between 3 and 5000 characters long")]
        public string Problem { get; set; }
    }
}
