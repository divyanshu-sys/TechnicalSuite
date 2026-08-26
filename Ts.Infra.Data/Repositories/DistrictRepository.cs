using Microsoft.EntityFrameworkCore;
using Ts.Domain.DataTableModels.DistrictDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public DistrictRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<District>> GetAllAsync(DistrictDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<District> orderQueriableEntity = null;
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

        public Task<List<District>> GetAllByStateIdAsync(int stateId)
        {
            return dbContext.Districts.Where(x => x.StateId == stateId).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(DistrictDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        private IQueryable<District> CommonSearch(DistrictDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Districts.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Name.Contains(requestModel.Search.Value));

            return queriableEntity;
        }
    }
}
