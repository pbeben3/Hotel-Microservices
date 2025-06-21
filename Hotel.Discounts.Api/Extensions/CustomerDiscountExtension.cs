using Hotel.Discounts.CrossCutting.Dtos;
using Hotel.Discounts.Storage.Entities;

namespace Hotel.Discounts.Api.Extensions
{
    public static class CustomerDiscountExtension
    {
        public static CustomerDiscountDto ToDto(this CustomerDiscount entity)
        {
            return new CustomerDiscountDto
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                DiscountId = entity.DiscountId,
                AssignedOn = entity.AssignedOn,
                IsActive = entity.IsActive,          
                Discount = entity.Discount?.ToDto() 
            };
        }
    }
}
