using Hotel.Common.Api.Service;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Reservations.Api.Controllers;
using Hotel.Reservations.Api.Extensions;
using Hotel.Reservations.Api.Resolvers;
using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage;
using Hotel.Reservations.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Reservations.Api.Services
{
   public class ReservationService : CrudServiceBase<ReservationDbContext, Reservation, ReservationDto>
    {
        private readonly ReservationDbContext _dbContext;
        private readonly CustomerIntegrationDataResolver _customerResolver;
        private readonly CustomerDiscountIntegrationDataResolver _discountResolver;

        public ReservationService(
            ReservationDbContext dbContext,
            CustomerIntegrationDataResolver customerResolver,
            CustomerDiscountIntegrationDataResolver discountResolver)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _customerResolver = customerResolver;
            _discountResolver = discountResolver;
        }

        protected override IQueryable<Reservation> ConfigureFormIncludes(IQueryable<Reservation> query)
        {
            return query
                .Include(r => r.Room)
                .Include(r => r.Customer)
                .Include(r => r.CustomerDiscount);
        }

        public async Task<IEnumerable<ReservationDto>> GetAll()
        {
            var entities = await base.Get();
            return entities.Select(r => r.ToDto());
        }

        public async Task<ReservationDto?> GetById(Guid id)
        {
            var entity = await base.GetById(id);
            return entity?.ToDto();
        }

        public async Task<IEnumerable<ReservationDto>> GetByCustomerId(Guid customerId)
        {
            await _customerResolver.ResolveFor(customerId);
            await _discountResolver.ResolveFor(customerId);

            var reservations = await _dbContext.Reservations
                .Where(r => r.CustomerId == customerId)
                .Include(r => r.Room)
                .Include(r => r.CustomerDiscount)
                .AsNoTracking()
                .ToListAsync();

            return reservations.Select(r => r.ToDto());
        }

        public async Task<CrudOperationResult<ReservationDto>> Create(CreateReservationRequest request)
        {
            // Pobierz lub synchronizuj klienta i zniżkę
            await _customerResolver.ResolveFor(request.CustomerId);
            var discount = await _discountResolver.ResolveFor(request.CustomerId);

            var room = await _dbContext.Rooms.FindAsync(request.RoomId);
            if (room == null)
                return new CrudOperationResult<ReservationDto> { Status = CrudOperationResultStatus.RecordNotFound };

            var nights = (request.EndDate.Date - request.StartDate.Date).Days;
            if (nights <= 0)
                return new CrudOperationResult<ReservationDto> { Status = CrudOperationResultStatus.Failure };

            decimal basePrice = room.PricePerNight * nights;
            decimal finalPrice = basePrice;

            if (discount != null)
            {
                if (discount.Type.Equals("procentowa", StringComparison.OrdinalIgnoreCase))
                    finalPrice = basePrice * (1 - discount.Value / 100m);
                else if (discount.Type.Equals("kwotowa", StringComparison.OrdinalIgnoreCase))
                    finalPrice = basePrice - discount.Value;

                finalPrice = Math.Max(finalPrice, 0);
            }

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = request.RoomId,
                CustomerId = request.CustomerId,
                CustomerDiscountId = discount?.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = ReservationStatus.Pending,
                PriceBeforeDiscount = basePrice,
                FinalPrice = Math.Round(finalPrice, 2)
            };

            var newId = await base.Create(reservation);
            await _dbContext.SaveChangesAsync();

            var newDto = await GetById(newId);
            return new CrudOperationResult<ReservationDto> { Status = CrudOperationResultStatus.Success, Result = newDto };
        }


        public async Task<CrudOperationResult<ReservationDto>> ConfirmReservation(Guid reservationId)
        {
            var reservation = await _dbContext.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return new CrudOperationResult<ReservationDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }
            if (reservation.Status != ReservationStatus.Pending)
            {
                return new CrudOperationResult<ReservationDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    Result = null
                };
            }
            reservation.Status = ReservationStatus.Confirmed;
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(reservation).Reference(r => r.Room).LoadAsync();
            await _dbContext.Entry(reservation).Reference(r => r.Customer).LoadAsync();
            if (reservation.CustomerDiscountId != null)
                await _dbContext.Entry(reservation).Reference(r => r.CustomerDiscount).LoadAsync();

            return new CrudOperationResult<ReservationDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = reservation.ToDto()
            };
        }

        public async Task<CrudOperationResult<ReservationDto>> CancelReservation(Guid reservationId)
        {
            var reservation = await _dbContext.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return new CrudOperationResult<ReservationDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }
            if (reservation.Status != ReservationStatus.Pending)
            {
                return new CrudOperationResult<ReservationDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    Result = null
                };
            }
            reservation.Status = ReservationStatus.Cancelled;
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(reservation).Reference(r => r.Room).LoadAsync();
            await _dbContext.Entry(reservation).Reference(r => r.Customer).LoadAsync();
            if (reservation.CustomerDiscountId != null)
                await _dbContext.Entry(reservation).Reference(r => r.CustomerDiscount).LoadAsync();

            return new CrudOperationResult<ReservationDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = reservation.ToDto()
            };
        }
    }
}
