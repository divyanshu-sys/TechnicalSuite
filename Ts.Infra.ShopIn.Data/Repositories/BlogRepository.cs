using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.DataTableModels.BlogDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        private readonly ShopInDbContext dbContext;

        public BlogRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Blog>> GetAllAsync(BlogDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<Blog> orderQueriableEntity = null;
            if (requestModel.OrderList.Any())
            {
                foreach (var item in requestModel.OrderList)
                {
                    if (item.OrderBy != null)
                        if (item.IsAsc)
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.IsPublished)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.IsPublished);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.BlogView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.BlogView.LastViewedOn);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.IsPublished)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.IsPublished);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.BlogView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.BlogView.LastViewedOn);
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
                                if (item.OrderBy.IsPublished)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.IsPublished);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.BlogView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.BlogView.LastViewedOn);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.IsPublished)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.IsPublished);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.BlogView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.BlogView.LastViewedOn);
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Blog
            {
                Id = x.Id,
                Title = x.Title,
                BlogLink = x.BlogLink,
                SubCategoryId = x.SubCategoryId,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                PublishedById = x.PublishedById,
                BlogWorkerId = x.BlogWorkerId,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                BlogView = x.BlogView
            }).ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(BlogDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).CountAsync();
        }

        public Task<List<Blog>> GetForListViewAsync(BlogDataTableForViewRequest requestModel)
        {
            var queryable = dbContext.Blogs.AsQueryable();

            if (!string.IsNullOrEmpty(requestModel.Search))
                queryable = queryable.Where(x => x.Title.Contains(requestModel.Search)
                || x.Keyword1.Contains(requestModel.Search)
                || x.Keyword2.Contains(requestModel.Search));

            if (requestModel.SubCategoryId.HasValue)
                queryable = queryable.Where(x => x.SubCategoryId == requestModel.SubCategoryId.Value);

            queryable = queryable.Where(x => x.IsPublished).OrderByDescending(x => x.PublishedOn);

            return queryable.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Blog
            {
                Id = x.Id,
                Title = x.Title,
                MainImage = x.MainImage,
                BlogLink = x.BlogLink,
                SubCategoryId = x.SubCategoryId,
                PublishedOn = x.PublishedOn,
                UpdatedOn = x.UpdatedOn
            }).ToListAsync();
        }

        private IQueryable<Blog> CommonSearch(BlogDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Blogs.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Title.Contains(requestModel.Search.Value)
                || x.BlogLink == requestModel.Search.Value);

            if (!string.IsNullOrEmpty(requestModel.BlogWorkerId))
                queriableEntity = queriableEntity.Where(x => x.BlogWorkerId == requestModel.BlogWorkerId);

            if (requestModel.LastViewedOnStart.HasValue)
                queriableEntity = queriableEntity.Where(x => x.BlogView.LastViewedOn >= requestModel.LastViewedOnStart.Value);

            if (requestModel.LastViewedOnEnd.HasValue)
                queriableEntity = queriableEntity.Where(x => x.BlogView.LastViewedOn < requestModel.LastViewedOnEnd.Value);

            return queriableEntity;
        }

        public Task<Blog> GetForViewAsync(int subCategoryId, string blogLink)
        {
            return dbContext.Blogs.SingleOrDefaultAsync(x => x.SubCategoryId == subCategoryId && x.BlogLink == blogLink && x.IsPublished);
        }

        public Task<List<BlogView>> GetBlogViewWithLockByBlogIdsAsync(IEnumerable<int> blogIds)
        {
            return dbContext.BlogViews.FromSqlRaw("select * from BlogViews with(rowlock, updlock, holdlock)")
                            .Where(x => blogIds.Contains(x.Id))
                            .ToListAsync();
        }

        public void AddBlogView(BlogView entity)
        {
            dbContext.BlogViews.Add(entity);
        }

        public Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null)
        {
            var query = dbContext.BlogViews.AsQueryable();

            if (lastViewedOnStart.HasValue)
                query = query.Where(x => x.LastViewedOn >= lastViewedOnStart.Value.UtcDateTime);

            if (lastViewedOnEnd.HasValue)
                query = query.Where(x => x.LastViewedOn < lastViewedOnEnd.Value.UtcDateTime);

            return query.CountAsync();
        }

        public Task<int> GetTotalPagesVisitedLifetimeAsync()
        {
            return dbContext.BlogViews.SumAsync(x => x.TotalViews);
        }
    }
}
