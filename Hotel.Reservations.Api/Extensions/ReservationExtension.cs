using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Extensions
{
    public static class ReservationExtension
    {
        public static ReservationDto ToDto(this Reservation entity)
        {
            return new ReservationDto
            {
                Id = entity.Id,
                RoomId = entity.RoomId,
                Room = entity.Room != null ? entity.Room.ToDto() : null,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer != null ? entity.Customer.ToDto() : null,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status.ToString(),
                CustomerDiscountId = entity.CustomerDiscountId,
                CustomerDiscount = entity.CustomerDiscount != null ? entity.CustomerDiscount.ToDto() : null,
                PriceBeforeDiscount = entity.PriceBeforeDiscount,
                FinalPrice = entity.FinalPrice
            };
        }
    }
}
