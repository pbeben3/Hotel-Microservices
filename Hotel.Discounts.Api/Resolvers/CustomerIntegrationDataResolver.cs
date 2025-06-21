using Hotel.Discounts.Storage;
using Hotel.Discounts.Storage.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Discounts.Api.Resolvers
{
    public class CustomerIntegrationDataResolver
    {
        private readonly DiscountDbContext _dbContext;

        public CustomerIntegrationDataResolver(DiscountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ResolveFor(Guid customerId)
        {
            var exists = await _dbContext.Customers.AnyAsync(x => x.Id == customerId);

            if (!exists)
            {
                var customerDto = await ResolveFromExternalDictionary(customerId);
                if (customerDto is not null)
                {
                    await CreateOrUpdateCustomer(customerDto);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task<ExternalCustomerDto?> ResolveFromExternalDictionary(Guid customerId)
        {
            const string apiUrl = "http://localhost:4371/api/customers/";

            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"{customerId}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var customerDto = JsonConvert.DeserializeObject<ExternalCustomerDto>(json);

            return customerDto;
        }

        private async Task<Customer> CreateOrUpdateCustomer(ExternalCustomerDto dto)
        {
            var customer = new Customer
            {
                Id = dto.Id,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Email = dto.Email,
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

        }
    }
}

