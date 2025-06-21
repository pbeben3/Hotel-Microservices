using Hotel.Customers.Api.Services;
using Hotel.Customers.Storage;

namespace Hotel.Customers.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CustomerService>();
            serviceCollection.AddDbContext<CustomerDbContext, CustomerDbContext>();
            return serviceCollection;
        }
    }
}
