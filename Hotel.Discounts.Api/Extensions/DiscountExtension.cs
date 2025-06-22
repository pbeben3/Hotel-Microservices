using Hotel.Discounts.CrossCutting.Dtos;
using Hotel.Discounts.Storage.Entities;

namespace Hotel.Discounts.Api.Extensions
{
    public static class DiscountExtension
    {
        public static DiscountDto ToDto(this Discount entity)
        {
            var result = new DiscountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Value = entity.Value,
                Type = entity.Type.ToString()
            };
            return result;
        }
    }
}
