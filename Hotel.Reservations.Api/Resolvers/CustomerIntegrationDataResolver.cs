using Hotel.Reservations.Storage;
using Hotel.Reservations.Storage.Entities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Hotel.Reservations.Api.Resolvers
{
    public class CustomerIntegrationDataResolver
    {
        private readonly ReservationDbContext _dbContext;

        public CustomerIntegrationDataResolver(ReservationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ResolveFor(Guid customerId)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                var dto = await ResolveFromExternalApi(customerId);
                if (dto != null)
                {
                    await CreateOrUpdateCustomer(dto);
                    await _dbContext.SaveChangesAsync();
                }
            }

        }

        private async Task<ExternalCustomerDto?> ResolveFromExternalApi(Guid customerId)
        {
            const string apiUrl = "http://localhost:4371/api/customers/";

            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"{customerId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<ExternalCustomerDto>(json);

            return dto;
        }

        private async Task<Customer> CreateOrUpdateCustomer(ExternalCustomerDto dto)
        {
            var customer = new Customer
            {
                Id = dto.Id,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                ExternalId = dto.Id.ToString(),
                ExternalSourceName = "CustomerAPI",
                LastSynchronizedOn = DateTime.UtcNow
            };

            _dbContext.Customers.Add(customer);
            return customer;

        }

        public class ExternalCustomerDto
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string? PhoneNumber { get; set; }
        }
    }
}
