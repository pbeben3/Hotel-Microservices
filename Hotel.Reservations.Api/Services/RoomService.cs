using Hotel.Common.Api.Service;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Reservations.Api.Extensions;
using Hotel.Reservations.CrossCutting.Dtos;
using Hotel.Reservations.Storage;
using Hotel.Reservations.Storage.Entities;

namespace Hotel.Reservations.Api.Services
{
    public class RoomService : CrudServiceBase<ReservationDbContext, Room, RoomDto>
    {
        private readonly ReservationDbContext _dbContext;

        public RoomService(ReservationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override IQueryable<Room> ConfigureFormIncludes(IQueryable<Room> query)
        {
            return query;
        }

        public async Task<RoomDto?> GetById(Guid id)
        {
            var entity = await base.GetById(id);
            return entity?.ToDto();
        }

        public async Task<IEnumerable<RoomDto>> Get()
        {
            var entities = await base.Get();
            return entities.Select(r => r.ToDto());
        }

        public async Task<CrudOperationResult<RoomDto>> Create(RoomDto dto)
        {
            var entity = dto.ToEntity();

            var newId = await base.Create(entity);
            await _dbContext.SaveChangesAsync();

            var newDto = await GetById(newId);

            return new CrudOperationResult<RoomDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = newDto
            };
        }

        public async Task<CrudOperationResult<RoomDto>> Update(RoomDto dto)
        {
            var entity = dto.ToEntity();
            return await base.Update(entity);
        }

        public async Task<CrudOperationResult<RoomDto>> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
