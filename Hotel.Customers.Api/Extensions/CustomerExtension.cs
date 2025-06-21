using Hotel.Customers.CrossCutting.Dtos;
using Hotel.Customers.Storage.Entities;

namespace Hotel.Customers.Api.Extensions
{
    public static class CustomerExtension
    {
        public static CustomerDto ToDto(this Customer entity)
        {
            var result = new CustomerDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
            };
            return result;
        }

    }
}
