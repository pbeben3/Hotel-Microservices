using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Extensions
{
    public static class RoomDtoExtension
    {

        public static Room ToEntity(this RoomDto dto)
        {
            return new Room
            {
                Id = dto.Id, 
                Name = dto.Name,
                Capacity = dto.Capacity,
                PricePerNight = dto.PricePerNight
            };
        }
    }
}
