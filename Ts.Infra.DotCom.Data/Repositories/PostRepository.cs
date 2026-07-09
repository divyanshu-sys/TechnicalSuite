using Microsoft.EntityFrameworkCore;
using Ts.DotCom.Domain.DataTableModels.PostDataTables;
using Ts.DotCom.Domain.Interfaces;
using Ts.DotCom.Domain.Models;
using Ts.Infra.DotCom.Data.Context;
namespace Ts.Infra.DotCom.Data.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly DotComDbContext dbContext;

        public PostRepository(DotComDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Post>> GetAllAsync(PostDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<Post> orderQueriableEntity = null;
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
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.PostView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.PostView.LastViewedOn);
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
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.PostView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.PostView.LastViewedOn);
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
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.PostView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.PostView.LastViewedOn);
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
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.PostView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.PostView.LastViewedOn);
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Post
            {
                Id = x.Id,
                Title = x.Title,
                PostLink = x.PostLink,
                CategoryId = x.CategoryId,
                SubCategoryId = x.SubCategoryId,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                PublishedById = x.PublishedById,
                PostWorkerId = x.PostWorkerId,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                PostView = x.PostView
            }).ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(PostDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).CountAsync();
        }

        public Task<List<Post>> GetForListViewAsync(PostDataTableForViewRequest requestModel)
        {
            var queryable = dbContext.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(requestModel.Search))
                queryable = queryable.Where(x => x.Title.Contains(requestModel.Search)
                || x.Keyword1.Contains(requestModel.Search)
                || x.Keyword2.Contains(requestModel.Search)
                || x.Keyword3.Contains(requestModel.Search)
                || x.Keyword4.Contains(requestModel.Search)
                || x.Keyword5.Contains(requestModel.Search));

            if (requestModel.CategoryId.HasValue)
                queryable = queryable.Where(x => x.CategoryId == requestModel.CategoryId.Value);

            if (requestModel.SubCategoryId.HasValue)
                queryable = queryable.Where(x => x.SubCategoryId == requestModel.SubCategoryId.Value);

            queryable = queryable.Where(x => x.IsPublished).OrderByDescending(x => x.PublishedOn);

            return queryable.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Post
            {
                Id = x.Id,
                Title = x.Title,
                MainImage = x.MainImage,
                PostLink = x.PostLink,
                CategoryId = x.CategoryId,
                SubCategoryId = x.SubCategoryId,
                PublishedOn = x.PublishedOn,
                UpdatedOn = x.UpdatedOn
            }).ToListAsync();
        }

        private IQueryable<Post> CommonSearch(PostDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Posts.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Title.Contains(requestModel.Search.Value)
                || x.PostLink == requestModel.Search.Value);

            if (!string.IsNullOrEmpty(requestModel.PostWorkerId))
                queriableEntity = queriableEntity.Where(x => x.PostWorkerId == requestModel.PostWorkerId);

            if (requestModel.LastViewedOnStart.HasValue)
                queriableEntity = queriableEntity.Where(x => x.PostView.LastViewedOn >= requestModel.LastViewedOnStart.Value);

            if (requestModel.LastViewedOnEnd.HasValue)
                queriableEntity = queriableEntity.Where(x => x.PostView.LastViewedOn < requestModel.LastViewedOnEnd.Value);

            return queriableEntity;
        }

        public Task<Post> GetForViewAsync(int categoryId, int subCategoryId, string postLink)
        {
            return dbContext.Posts.SingleOrDefaultAsync(x => x.CategoryId == categoryId && x.SubCategoryId == subCategoryId && x.PostLink == postLink && x.IsPublished);
        }

        public Task<List<PostView>> GetPostViewWithLockByPostIdsAsync(IEnumerable<int> postIds)
        {
            return dbContext.PostViews.FromSqlRaw("select * from PostViews with(rowlock, updlock, holdlock)")
                            .Where(x => postIds.Contains(x.Id))
                            .ToListAsync();
        }

        public void AddPostView(PostView entity)
        {
            dbContext.PostViews.Add(entity);
        }

        public Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null)
        {
            var query = dbContext.PostViews.AsQueryable();

            if (lastViewedOnStart.HasValue)
                query = query.Where(x => x.LastViewedOn >= lastViewedOnStart.Value.UtcDateTime);

            if (lastViewedOnEnd.HasValue)
                query = query.Where(x => x.LastViewedOn < lastViewedOnEnd.Value.UtcDateTime);

            return query.CountAsync();
        }

        public Task<int> GetTotalPagesVisitedLifetimeAsync()
        {
            return dbContext.PostViews.SumAsync(x => x.TotalViews);
        }
    }
}
