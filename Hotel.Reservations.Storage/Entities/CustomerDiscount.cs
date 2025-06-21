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
    [Table("CustomerDiscounts", Schema = "Dictionaries")]
    public class CustomerDiscount : BaseEntity, IExternalSourceEntity
    {
        [Required]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [Required]
        public Guid DiscountId { get; set; }

        [Required, MaxLength(100)]
        public string DiscountName { get; set; } = null!;

        [Required]
        public decimal Value { get; set; }

        [Required, MaxLength(20)]
        public string Type { get; set; } = "procentowa";

        public DateTime AssignedOn { get; set; }

        public string ExternalSourceName { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public DateTime? LastSynchronizedOn { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }
}
