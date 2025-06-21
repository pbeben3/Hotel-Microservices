using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Common.Storage.Entities
{
    public interface IExternalSourceEntity
    {
        [MaxLength(5)]
        string? ExternalSourceName { get; set; }
        [MaxLength(38)]
        string? ExternalId { get; set; }
        public DateTime? LastSynchronizedOn { get; set; }
    }
}
