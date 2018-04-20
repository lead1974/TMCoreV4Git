using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Models.Customer
{
    public class CustomerCoupon
    {
        [Key]
        [Required]
        public int CustomerCouponId { get; set; }

        [Display(Name = "Coupon Title")]
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Coupon Description")]
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Display(Name = "Coupon Expiration Date")]
        [DataType(DataType.Text)]
        public DateTime? ExpirationDate { get; set; }
        public int Sequence { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

    }
}
