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
    [Table("Customers", Schema = "Dictionaries")]

    public class Customer : BaseEntity, IExternalSourceEntity
    {
        [MaxLength(256)]
        public string FullName { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        public string ExternalSourceName { get; set; }
        public string ExternalId { get; set; }
        public DateTime? LastSynchronizedOn { get; set; }

        public ICollection<CustomerDiscount> Discounts { get; set; } = new HashSet<CustomerDiscount>();
    }
}
