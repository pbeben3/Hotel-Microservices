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
    [Table("Customers", Schema = "Dictionaries")]
    public class Customer : BaseEntity, IExternalSourceEntity
    {
        [Required, MaxLength(256)]
        public string FullName { get; set; } = null!;

        [Required, MaxLength(256)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public string ExternalSourceName { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public DateTime? LastSynchronizedOn { get; set; }

        public ICollection<CustomerDiscount> Discounts { get; set; } = new HashSet<CustomerDiscount>();
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }
}
