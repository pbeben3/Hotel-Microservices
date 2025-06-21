using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Discounts.CrossCutting.Dtos
{
    public class DiscountDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Value { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = "Percentage"; 


    }
}
