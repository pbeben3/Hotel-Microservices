using Hotel.Discounts.CrossCutting.Dtos;
using Hotel.Discounts.Storage.Entities;

namespace Hotel.Discounts.Api.Extensions
{
    public static class DiscountDtoExtension
    {
        public static Discount ToEntity(this DiscountDto entity) 
        {
            var result = new Discount
            {
                Id = entity.Id,
                Name = entity.Name,
                Value = entity.Value,
                Type = entity.Type,
            };
            return result;
        }
    }
}
