using Microsoft.EntityFrameworkCore;
using Ts.DotIn.Domain.DataTableModels.StoryDataTables;
using Ts.DotIn.Domain.Interfaces;
using Ts.DotIn.Domain.Models;
using Ts.Infra.DotIn.Data.Context;
namespace Ts.Infra.DotIn.Data.Repositories
{
    public class StoryRepository : GenericRepository<Story>, IStoryRepository
    {
        private readonly DotInDbContext dbContext;

        public StoryRepository(DotInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Story>> GetAllAsync(StoryDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<Story> orderQueriableEntity = null;
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
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.StoryView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.StoryView.LastViewedOn);
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
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.StoryView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.StoryView.LastViewedOn);
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
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.StoryView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.StoryView.LastViewedOn);
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
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.StoryView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.StoryView.LastViewedOn);
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Story
            {
                Id = x.Id,
                Title = x.Title,
                StoryLink = x.StoryLink,
                SubCategoryId = x.SubCategoryId,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                PublishedById = x.PublishedById,
                StoryWorkerId = x.StoryWorkerId,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                StoryView = x.StoryView
            }).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(StoryDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        public Task<List<Story>> GetForListViewAsync(StoryDataTableForViewRequest requestModel)
        {
            var queryable = dbContext.Stories.AsQueryable();

            if (!string.IsNullOrEmpty(requestModel.Search))
                queryable = queryable.Where(x => x.Title.Contains(requestModel.Search)
                || x.Keyword1.Contains(requestModel.Search)
                || x.Keyword2.Contains(requestModel.Search));

            if (requestModel.SubCategoryId.HasValue)
                queryable = queryable.Where(x => x.SubCategoryId == requestModel.SubCategoryId.Value);

            queryable = queryable.Where(x => x.IsPublished).OrderByDescending(x => x.PublishedOn);

            return queryable.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Story
            {
                Id = x.Id,
                Title = x.Title,
                StoryLink = x.StoryLink,
                MainImage = x.MainImage,
                SubCategoryId = x.SubCategoryId,
                PublishedOn = x.PublishedOn,
                UpdatedOn = x.UpdatedOn
            }).AsNoTracking().ToListAsync();
        }

        private IQueryable<Story> CommonSearch(StoryDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Stories.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Title.Contains(requestModel.Search.Value)
                || x.StoryLink == requestModel.Search.Value);

            if (!string.IsNullOrEmpty(requestModel.StoryWorkerId))
                queriableEntity = queriableEntity.Where(x => x.StoryWorkerId == requestModel.StoryWorkerId);

            return queriableEntity;
        }

        public Task<Story> GetForViewAsync(int subCategoryId, string storyLink)
        {
            return dbContext.Stories.AsNoTracking().SingleOrDefaultAsync(x => x.SubCategoryId == subCategoryId && x.StoryLink == storyLink && x.IsPublished);
        }

        public Task<List<StoryView>> GetStoryViewWithLockByStoryIdsAsync(IEnumerable<int> storyIds)
        {
            return dbContext.StoryViews.FromSqlRaw("select * from StoryViews with(rowlock, updlock, holdlock)")
                            .Where(x => storyIds.Contains(x.Id))
                            .ToListAsync();
        }

        public void AddStoryView(StoryView entity)
        {
            dbContext.StoryViews.Add(entity);
        }
    }
}
