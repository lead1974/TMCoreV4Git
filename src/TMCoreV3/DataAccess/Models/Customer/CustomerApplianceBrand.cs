using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Models.Customer
{
    public class CustomerApplianceBrand
    {
        [Key]
        [Required]
        public int CustomerApplianceBrandId { get; set; }

        [Display(Name = "Appliance Brand")]
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public int Sequence { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        
        public int CustomerApplianceTypeId { get; set; }
        public virtual CustomerApplianceType customerApplianceType { get; set; }

    }
}
