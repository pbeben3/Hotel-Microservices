using Hotel.Common.Api.Service;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Customers.CrossCutting.Dtos;
using Hotel.Customers.Storage;
using Hotel.Customers.Storage.Entities;
using Hotel.Customers.Api.Extensions;


namespace Hotel.Customers.Api.Services
{
    public class CustomerService : CrudServiceBase<CustomerDbContext, Customer, CustomerDto>
    {
        private readonly CustomerDbContext _dbContext;

        public CustomerService(CustomerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override IQueryable<Customer> ConfigureFormIncludes(IQueryable<Customer> linq)
        {
            return linq;
        }

        public async Task<CustomerDto?> GetById(Guid id)
        {
            var customer = await base.GetById(id);
            return customer?.ToDto();
        }

        public async Task<IEnumerable<CustomerDto>> Get()
        {
            var customers = await base.Get();
            return customers.Select(c => c.ToDto());
        }

        public async Task<CrudOperationResult<CustomerDto>> Create(CustomerDto dto)
        {
            var entity = dto.ToEntity();

            var newId = await base.Create(entity);

            var newDto = await GetById(newId);

            return new CrudOperationResult<CustomerDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = newDto
            };
        }

        public async Task<CrudOperationResult<CustomerDto>> Update(CustomerDto dto)
        {
            var entity = dto.ToEntity();
            return await base.Update(entity);
        }

        public async Task<CrudOperationResult<CustomerDto>> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
