using Hotel.Discounts.CrossCutting.Dtos;
using Hotel.Discounts.Storage.Entities;

namespace Hotel.Discounts.Api.Extensions
{
    public static class CustomerDiscountDtoExtension
    {
        public static CustomerDiscount ToEntity(this CustomerDiscountDto entity)
        {
            var result = new CustomerDiscount
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                DiscountId = entity.DiscountId,
                AssignedOn = entity.AssignedOn,
                IsActive = entity.IsActive 
            };
            return result;
        }
    }
}
