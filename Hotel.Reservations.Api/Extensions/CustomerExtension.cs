using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Extensions
{
    public static class CustomerExtension
    {
        public static CustomerDto ToDto(this Customer entity)
        {
            return new CustomerDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };
        }
    }
}
