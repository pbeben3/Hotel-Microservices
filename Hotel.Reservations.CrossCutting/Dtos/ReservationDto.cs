using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservations.CrossCutting.Dtos
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public RoomDto? Room { get; set; }  

        public Guid CustomerId { get; set; }
        public CustomerDto? Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = null!; 

        public Guid? CustomerDiscountId { get; set; }
        public CustomerDiscountDto? CustomerDiscount { get; set; }

        public decimal PriceBeforeDiscount { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
