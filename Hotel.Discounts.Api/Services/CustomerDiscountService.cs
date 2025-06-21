using Hotel.Common.Api.Service;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Discounts.Api.Extensions;
using Hotel.Discounts.Api.Resolvers;
using Hotel.Discounts.CrossCutting.Dtos;
using Hotel.Discounts.Storage;
using Hotel.Discounts.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Discounts.Api.Services
{
    public class CustomerDiscountService : CrudServiceBase<DiscountDbContext, CustomerDiscount, CustomerDiscountDto>
    {
        private readonly DiscountDbContext _dbContext;
        private readonly CustomerIntegrationDataResolver _customerResolver;

        public CustomerDiscountService(DiscountDbContext dbContext, CustomerIntegrationDataResolver customerResolver)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _customerResolver = customerResolver;
        }

        protected override IQueryable<CustomerDiscount> ConfigureFormIncludes(IQueryable<CustomerDiscount> query)
        {
            return query.Include(cd => cd.Discount);
        }


        public async Task<IEnumerable<CustomerDiscountDto>> GetByCustomerId(Guid customerId)
        {
            await _customerResolver.ResolveFor(customerId);

            var list = await _dbContext.CustomerDiscounts
                .Where(cd => cd.CustomerId == customerId)
                .Include(cd => cd.Discount)
                .AsNoTracking()
                .ToListAsync();

            return list.Select(cd => cd.ToDto());
        }


        public async Task<CustomerDiscountDto?> GetActiveForCustomer(Guid customerId)
        {
            await _customerResolver.ResolveFor(customerId);

            var active = await _dbContext.CustomerDiscounts
                .Where(cd => cd.CustomerId == customerId && cd.IsActive)
                .Include(cd => cd.Discount)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return active?.ToDto();
        }


        public async Task<CrudOperationResult<CustomerDiscountDto>> AssignDiscountToCustomer(Guid customerId, Guid discountId)
        {
            await _customerResolver.ResolveFor(customerId);

            var customerExists = await _dbContext.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists)
            {
                return new CrudOperationResult<CustomerDiscountDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }

            var discount = await _dbContext.Discounts.FindAsync(discountId);
            if (discount == null)
            {
                return new CrudOperationResult<CustomerDiscountDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }

            var now = DateTime.UtcNow;
            var customerDiscount = new CustomerDiscount
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                DiscountId = discountId,
                AssignedOn = now,
                IsActive = false
            };

            _dbContext.CustomerDiscounts.Add(customerDiscount);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(customerDiscount).Reference(cd => cd.Discount).LoadAsync();

            return new CrudOperationResult<CustomerDiscountDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = customerDiscount.ToDto()
            };
        }


        public async Task<CrudOperationResult<CustomerDiscountDto>> SetActive(Guid customerDiscountId)
        {
            var cd = await _dbContext.CustomerDiscounts.FindAsync(customerDiscountId);
            if (cd == null)
            {
                return new CrudOperationResult<CustomerDiscountDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }

            var otherActives = await _dbContext.CustomerDiscounts
                .Where(x => x.CustomerId == cd.CustomerId && x.IsActive && x.Id != customerDiscountId)
                .ToListAsync();

            foreach (var other in otherActives)
            {
                other.IsActive = false;
            }

            cd.IsActive = true;

            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(cd).Reference(x => x.Discount).LoadAsync();

            return new CrudOperationResult<CustomerDiscountDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = cd.ToDto()
            };
        }


        public async Task<CrudOperationResult<CustomerDiscountDto>> SetInactive(Guid customerDiscountId)
        {
            var cd = await _dbContext.CustomerDiscounts.FindAsync(customerDiscountId);
            if (cd == null)
            {
                return new CrudOperationResult<CustomerDiscountDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }


            cd.IsActive = false;
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(cd).Reference(x => x.Discount).LoadAsync();

            return new CrudOperationResult<CustomerDiscountDto>
            {
                Status = CrudOperationResultStatus.Success,
                Result = cd.ToDto()
            };
        }

        public async Task<CrudOperationResult<object>> DeleteAssignment(Guid customerDiscountId)
        {
            var cd = await _dbContext.CustomerDiscounts.FindAsync(customerDiscountId);
            if (cd == null)
            {
                return new CrudOperationResult<object>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                    Result = null
                };
            }

            _dbContext.CustomerDiscounts.Remove(cd);
            await _dbContext.SaveChangesAsync();

            return new CrudOperationResult<object>
            {
                Status = CrudOperationResultStatus.Success,
                Result = null
            };
        }
    }
}
