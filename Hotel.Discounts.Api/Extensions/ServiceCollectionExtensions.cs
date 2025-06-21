using Hotel.Discounts.Api.Resolvers;
using Hotel.Discounts.Api.Services;
using Hotel.Discounts.Storage;

namespace Hotel.Discounts.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscountService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<DiscountService>();
            serviceCollection.AddTransient<CustomerDiscountService>();
            serviceCollection.AddTransient<CustomerIntegrationDataResolver>();
            serviceCollection.AddDbContext<DiscountDbContext, DiscountDbContext>();
            return serviceCollection;
        }
    }
}
