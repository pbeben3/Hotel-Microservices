using Hotel.Common.Api.Service;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Discounts.CrossCutting.Dtos;
using Hotel.Discounts.Storage.Entities;
using Hotel.Discounts.Api.Extensions;
using Hotel.Discounts.Storage;

namespace Hotel.Discounts.Api.Services
{
    public class DiscountService : CrudServiceBase<DiscountDbContext, Discount, DiscountDto>
    {
        private readonly DiscountDbContext _dbContext;

        public DiscountService(DiscountDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override IQueryable<Discount> ConfigureFormIncludes(IQueryable<Discount> linq)
        {
            return linq;
        }

        public async Task<DiscountDto?> GetById(Guid id)
        {
            var entity = await base.GetById(id);
            return entity?.ToDto();
        }

        public async Task<IEnumerable<DiscountDto>> Get()
        {
            var entities = await base.Get();
            return entities.Select(d => d.ToDto());
        }

        public async Task<CrudOperationResult<DiscountDto>> Create(DiscountDto dto)
        {
            var entity = dto.ToEntity();

            var newId = await base.Create(entity);
            await _dbContext.SaveChangesAsync();

            var newDto = await GetById(newId);

            return new CrudOperationResult<DiscountDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = newDto
            };
        }

        public async Task<CrudOperationResult<DiscountDto>> Update(DiscountDto dto)
        {
            var entity = dto.ToEntity();
            return await base.Update(entity);
        }

        public async Task<CrudOperationResult<DiscountDto>> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
