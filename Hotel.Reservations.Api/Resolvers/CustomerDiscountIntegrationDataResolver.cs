using Hotel.Reservations.Storage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Resolvers
{
    public class CustomerDiscountIntegrationDataResolver
    {
        private readonly ReservationDbContext _dbContext;

        public CustomerDiscountIntegrationDataResolver(ReservationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerDiscount?> ResolveFor(Guid customerId)
        {
            var externalDiscount = await ResolveFromExternalApi(customerId);

            if (externalDiscount == null)
                return null;


            var existing = await _dbContext.CustomerDiscounts
                .FirstOrDefaultAsync(d => d.Id == externalDiscount.Id);

            if (existing == null)
            {
                var entity = await CreateOrUpdateCustomerDiscount(externalDiscount);
                await _dbContext.SaveChangesAsync();
                return entity;
            }

            return existing;
        }

        private async Task<ExternalCustomerDiscountDto?> ResolveFromExternalApi(Guid customerId)
        {
            const string apiUrl = "http://localhost:5154";

            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"/api/customer-discounts/customer/{customerId}/active");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dtos = JsonConvert.DeserializeObject<ExternalCustomerDiscountDto>(json);

            return dtos;
        }

        private async Task<CustomerDiscount> CreateOrUpdateCustomerDiscount(ExternalCustomerDiscountDto dto)
        {
            var customerDiscount = new CustomerDiscount
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                DiscountId = dto.DiscountId,
                DiscountName = dto.Discount.Name,
                Value = dto.Discount.Value,
                Type = dto.Discount.Type,
                AssignedOn = dto.AssignedOn,
                ExternalId = dto.Id.ToString(),
                ExternalSourceName = "CustomerDiscountAPI",
                LastSynchronizedOn = DateTime.UtcNow
            };
            _dbContext.CustomerDiscounts.Add(customerDiscount);
            return customerDiscount;
        }

        public class ExternalCustomerDiscountDto
        {
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid DiscountId { get; set; }
            public DiscountDto Discount { get; set; } = null!;
            public DateTime AssignedOn { get; set; }
        }

        public class DiscountDto
        {
            public string Name { get; set; } = null!;
            public decimal Value { get; set; }
            public string Type { get; set; } = null!;
        }
    }
}
