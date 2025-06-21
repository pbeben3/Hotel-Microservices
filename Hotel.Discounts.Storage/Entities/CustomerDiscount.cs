using Hotel.Common.Storage.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Discounts.Storage.Entities
{
    [Table("CustomerDiscounts", Schema = "Discount")]
    public class CustomerDiscount : BaseEntity
    {
        [Required]
        public Guid CustomerId { get; set; } 

        [Required]
        public Guid DiscountId { get; set; } 

        public DateTime AssignedOn { get; set; }
        public bool IsActive { get; set; } = true;

        public Customer Customer { get; set; }
        public Discount Discount { get; set; }

    }
}
