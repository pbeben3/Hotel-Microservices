using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Discounts.CrossCutting.Dtos
{
    public class CustomerDiscountDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid DiscountId { get; set; }

        public DateTime AssignedOn { get; set; }
        public bool IsActive { get; set; }  
        public DiscountDto? Discount { get; set; }

    }
}
