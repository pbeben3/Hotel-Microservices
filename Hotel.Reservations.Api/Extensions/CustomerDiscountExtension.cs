using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Extensions
{
    public static class CustomerDiscountExtension
    {
        public static CustomerDiscountDto ToDto(this CustomerDiscount entity)
        {
            return new CustomerDiscountDto
            {
                Id = entity.Id,
                DiscountName = entity.DiscountName,
                Value = entity.Value,
                Type = entity.Type,
                AssignedOn = entity.AssignedOn
            };
        }
    }
}
