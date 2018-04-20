using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Models.Invoice
{
    public class Invoice
    {
        [Key]
        [Required]
        public int InvoiceId { get; set; }

        [Display(Name = "Invoice Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Invoice Name")]
        [Required]
        [DataType(DataType.Text)]
        public string InvoiceName { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email")]
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
        [DataType(DataType.Text)]
        public string PostalCode { get; set; }

        [Display(Name = "Customer BlackListed")]
        public bool CustomerBlackListed { get; set; }

        [Display(Name = "Work Notes")]
        [Required]
        [DataType(DataType.Text)]
        public string WorkNotes { get; set; }

        [Display(Name = "Charged Amount")]
        [Required]
        [DataType(DataType.Currency)]
        public float? ChargedAmount { get; set; }

        [Display(Name = "Tax Amount")]
        [DataType(DataType.Currency)]
        public float? TaxAmount { get; set; }

        [Display(Name = "Parts Notes")]
        [DataType(DataType.Text)]
        public string PartNotes { get; set; }

        [Display(Name = "Parts Paid")]
        [DataType(DataType.Currency)]
        public float? PartsPaid { get; set; }

        [Display(Name = "Warranty From Date")]
        [DataType(DataType.Date)]
        public DateTime WarrantyFromDate { get; set; }

        [Display(Name = "Warranty To Date")]
        [DataType(DataType.Date)]
        public DateTime WarrantyToDate { get; set; }

        [Display(Name = "Payment Type")]
        [DataType(DataType.Text)]
        public string PaymentType { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<FileUpload> FileUpload { get; set; }
    }
}
