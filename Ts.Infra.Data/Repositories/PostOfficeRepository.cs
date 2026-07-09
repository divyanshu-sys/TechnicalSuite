using Microsoft.EntityFrameworkCore;
using Ts.Domain.DataTableModels.PostOfficeDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class PostOfficeRepository : GenericRepository<PostOffice>, IPostOfficeRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public PostOfficeRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<PostOffice>> GetAllAsync(PostOfficeDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<PostOffice> orderQueriableEntity = null;
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(PostOfficeDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).CountAsync();
        }

        public Task<List<PostOffice>> GetAllByDistrictIdAsync(int districtId)
        {
            return dbContext.PostOffices.Where(x => x.DistrictId == districtId).ToListAsync();
        }

        private IQueryable<PostOffice> CommonSearch(PostOfficeDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.PostOffices.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Name.Contains(requestModel.Search.Value));
            return queriableEntity;
        }
    }
}
