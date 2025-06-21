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
    [Table("Discounts", Schema = "Discount")]
    public class Discount : BaseEntity
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!; 

        [Required]
        public decimal Value { get; set; } 

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = "Percentage"; 


        public virtual ICollection<CustomerDiscount> CustomerDiscounts { get; set; } = new List<CustomerDiscount>();
    }
}
