using Microsoft.EntityFrameworkCore;
using Ts.Domain.DataTableModels.CountryDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public CountryRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Country>> GetAllAsync(CountryDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<Country> orderQueriableEntity = null;
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
                                else if (item.OrderBy.Code2)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.Code2);
                                else if (item.OrderBy.Code3)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.Code3);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Name);
                                else if (item.OrderBy.Code2)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Code2);
                                else if (item.OrderBy.Code3)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Code3);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.UpdatedOn);
                            }
                        }
                        else
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Name);
                                else if (item.OrderBy.Code2)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Code2);
                                else if (item.OrderBy.Code3)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Code3);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Name);
                                else if (item.OrderBy.Code2)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Code2);
                                else if (item.OrderBy.Code3)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Code3);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.UpdatedOn);
                            }
                        }
                }
            }
            if (orderQueriableEntity != null)
                queriableEntity = orderQueriableEntity.AsQueryable();

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(CountryDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        private IQueryable<Country> CommonSearch(CountryDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Countries.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Name.Contains(requestModel.Search.Value)
                || x.Code2 == requestModel.Search.Value || x.Code3 == requestModel.Search.Value);

            return queriableEntity;
        }
    }
}
