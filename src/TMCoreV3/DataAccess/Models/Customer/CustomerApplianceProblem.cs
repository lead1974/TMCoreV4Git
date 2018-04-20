using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMCoreV3.DataAccess.Models.Customer;

namespace TMCoreV3.DataAccess.Models.Customer
{
    public class CustomerApplianceProblem
    {
        [Key]
        [Required]
        public int CustomerApplianceProblemId { get; set; }

        [Display(Name = "Appliance Problem")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "Appliance Problem must be between 3 and 5000 characters long")]
        public string Problem { get; set; }

        [Display(Name = "Model Number")]
        [Required]
        [DataType(DataType.Text)]
        public string ModelNumber { get; set; }

        [Display(Name = "Model Serial")]
        [Required]
        [DataType(DataType.Text)]
        public string ModelSerial { get; set; }

        [Display(Name = "Desired Schedule Time")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DesiredScheduleTime { get; set; }

        [Display(Name = "Actual Schedule Time")]
        [DataType(DataType.DateTime)]
        public DateTime? ActualScheduleTime { get; set; }

        [Display(Name = "Problem Status")]
        public string ProblemStatus { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        [Display(Name = "Date Issue Resolved")]
        public DateTime? DateResolved { get; set; }
        [Display(Name = "Issue Resolved By")]
        public string IssueResolvedBy { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int CustomerApplianceTypeId { get; set; }
        public virtual CustomerApplianceType CustomerApplianceType { get; set; }

        public int CustomerApplianceBrandId { get; set; }
        public virtual CustomerApplianceBrand CustomerApplianceBrand { get; set; }
    }
}
