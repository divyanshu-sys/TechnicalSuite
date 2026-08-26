using Microsoft.EntityFrameworkCore;
using Ts.Domain.DataTableModels.ShopCategoryDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class ShopCategoryRepository : GenericRepository<ShopCategory>, IShopCategoryRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public ShopCategoryRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<ShopCategory>> GetAllAsync(ShopCategoryDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<ShopCategory> orderQueriableEntity = null;
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
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Name);
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
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.Name)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Name);
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

        public Task<int> GetRecordsFilteredAsync(ShopCategoryDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        private IQueryable<ShopCategory> CommonSearch(ShopCategoryDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.ShopCategories.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Name.Contains(requestModel.Search.Value));

            return queriableEntity;
        }
    }
}
