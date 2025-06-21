using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservations.CrossCutting.Dtos
{
    public class CustomerDiscountDto
    {
        public Guid Id { get; set; }
        public string DiscountName { get; set; } = null!;
        public decimal Value { get; set; }
        public string Type { get; set; } = null!;
        public DateTime AssignedOn { get; set; }
    }
}
