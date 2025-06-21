using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Extensions
{
    public static class ReservationDtoExtension
    {

        public static Reservation ToEntity(this ReservationDto dto)
        {
            ReservationStatus status;
            if (!Enum.TryParse<ReservationStatus>(dto.Status, ignoreCase: true, out status))
            {
                status = ReservationStatus.Pending;
            }

            return new Reservation
            {
                Id = dto.Id, 
                RoomId = dto.RoomId,
                CustomerId = dto.CustomerId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = status,
                CustomerDiscountId = dto.CustomerDiscountId,
                PriceBeforeDiscount = dto.PriceBeforeDiscount,
                FinalPrice = dto.FinalPrice
            };
        }
    }
}
