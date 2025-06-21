using Hotel.Reservations.Api.Resolvers;
using Hotel.Reservations.Api.Services;
using Hotel.Reservations.Storage;

namespace Hotel.Reservations.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReservationService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<RoomService>();
            serviceCollection.AddTransient<ReservationService>();
            serviceCollection.AddTransient<CustomerDiscountIntegrationDataResolver>();
            serviceCollection.AddTransient<CustomerIntegrationDataResolver>();
            serviceCollection.AddDbContext<ReservationDbContext, ReservationDbContext>();
            return serviceCollection;
        }
    }
}
