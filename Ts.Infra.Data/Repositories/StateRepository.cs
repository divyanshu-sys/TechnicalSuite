using Microsoft.EntityFrameworkCore;
using Ts.Domain.DataTableModels.StateDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public StateRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<State>> GetAllAsync(StateDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<State> orderQueriableEntity = null;
            if (requestModel.OrderList.Any())
            {
                foreach (var item in requestModel.OrderList)
                {
                    if (item.OrderBy != null)
                        if (item.IsAsc)
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.Name);
                                if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.CreatedOn);
                                if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Name);
                                if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.CreatedOn);
                                if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.UpdatedOn);
                            }
                        }
                        else
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Name);
                                if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.CreatedOn);
                                if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Name);
                                if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.CreatedOn);
                                if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.UpdatedOn);
                            }
                        }
                }
            }
            if (orderQueriableEntity != null)
                queriableEntity = orderQueriableEntity.AsQueryable();

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).AsNoTracking().ToListAsync();
        }

        public Task<List<State>> GetAllByCountryIdAsync(int countryId)
        {
            return dbContext.States.Where(x => x.CountryId == countryId).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(StateDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        private IQueryable<State> CommonSearch(StateDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.States.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Name.Contains(requestModel.Search.Value)
                || x.Code2 == requestModel.Search.Value);

            return queriableEntity;
        }
    }
}
