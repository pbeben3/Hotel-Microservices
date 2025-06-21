using Hotel.Common.CrossCutting.Dtos;
using Hotel.Common.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Common.Api.Service
{
    public class CrudServiceBase<TDbContext, TEntity, TDto>
              where TDbContext : DbContext
              where TEntity : BaseEntity
              where TDto : class
    {
        private readonly TDbContext _dbContext;
        protected virtual Task OnBeforeRecordCreatedAsync(TDbContext dbContext, TEntity entity) => Task.CompletedTask;
        protected virtual Task OnAfterRecordCreatedAsync(TDbContext dbContext, TEntity entity) => Task.CompletedTask;

        public CrudServiceBase(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CrudOperationResult<TDto>> Delete(Guid id)
        {
            var entity = await _dbContext
                .Set<TEntity>()
                .SingleOrDefaultAsync(e => e.Id!.Equals(id));

            if (entity == null)
            {
                return new CrudOperationResult<TDto>()
                {
                    Status = CrudOperationResultStatus.RecordNotFound
                };
            }
            _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();
            return new CrudOperationResult<TDto>
            {
                Status = CrudOperationResultStatus.Success,
            };
        }

        public async Task<Guid> Create(TEntity entity)
        {
            await OnBeforeRecordCreatedAsync(_dbContext, entity);

            _dbContext
                .Set<TEntity>()
                .Add(entity);

            await _dbContext.SaveChangesAsync();

            await OnAfterRecordCreatedAsync(_dbContext, entity);

            return entity.Id;

        }

        public async Task<CrudOperationResult<TDto>> Update(TEntity newEntity)
        {
            var entityBeforeUpdate = await GetById(newEntity.Id);

            if (entityBeforeUpdate == null)
            {
                return new CrudOperationResult<TDto>
                {
                    Status = CrudOperationResultStatus.RecordNotFound,
                };
            }

            _dbContext.Entry(newEntity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return new CrudOperationResult<TDto>
            {
                Status = CrudOperationResultStatus.Success
            };
        }

        protected async virtual Task<TEntity> GetById(Guid id)
        {
            var query = _dbContext
                .Set<TEntity>()
                .AsNoTracking();

            var entity = await ConfigureFormIncludes(query)
                .SingleOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async virtual Task<IEnumerable<TEntity>> Get()
        {
            var query = _dbContext
                .Set<TEntity>()
                .AsNoTracking();

            var entities = await ConfigureFormIncludes(query)
                .ToListAsync();

            return entities;
        }

        protected virtual IQueryable<TEntity> ConfigureFormIncludes(IQueryable<TEntity> linq)
        {
            return linq;
        }
    }
}
