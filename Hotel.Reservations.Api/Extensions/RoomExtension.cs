using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Extensions
{
    public static class RoomExtension
    {
        public static RoomDto ToDto(this Room entity)
        {
            return new RoomDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Capacity = entity.Capacity,
                PricePerNight = entity.PricePerNight
            };
        }
    }
}
