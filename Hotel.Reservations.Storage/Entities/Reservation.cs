using Hotel.Common.Storage.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservations.Storage.Entities
{
    [Table("Reservations", Schema = "Reservation")]
    public class Reservation : BaseEntity
    {
        [Required]
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [Required]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public Guid? CustomerDiscountId { get; set; }
        public CustomerDiscount? CustomerDiscount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        [Required]
        public decimal PriceBeforeDiscount { get; set; }

        [Required]
        public decimal FinalPrice { get; set; }
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
