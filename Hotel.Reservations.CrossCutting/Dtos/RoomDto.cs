using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Reservations.CrossCutting.Dtos
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
