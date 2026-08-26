using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
    {
        private readonly ShopInDbContext dbContext;

        public ProductDetailRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<ProductDetail>> GetAllAsync(ProductDetailDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<ProductDetail> orderQueriableEntity = null;
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
                                else if (item.OrderBy.IsAvailable)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.IsAvailable);
                                else if (item.OrderBy.Stock)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.Stock);
                                else if (item.OrderBy.Mrp)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.Mrp);
                                else if (item.OrderBy.Price)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.Price);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.ProductDetailView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.ProductDetailView.LastViewedOn);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.IsPublished)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.IsPublished);
                                else if (item.OrderBy.IsAvailable)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.IsAvailable);
                                else if (item.OrderBy.Stock)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Stock);
                                else if (item.OrderBy.Mrp)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Mrp);
                                else if (item.OrderBy.Price)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.Price);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.ProductDetailView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.ProductDetailView.LastViewedOn);
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
                                else if (item.OrderBy.IsAvailable)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.IsAvailable);
                                else if (item.OrderBy.Stock)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Stock);
                                else if (item.OrderBy.Mrp)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Mrp);
                                else if (item.OrderBy.Price)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.Price);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.ProductDetailView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.ProductDetailView.LastViewedOn);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.IsPublished)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.IsPublished);
                                else if (item.OrderBy.IsAvailable)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.IsAvailable);
                                else if (item.OrderBy.Stock)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Stock);
                                else if (item.OrderBy.Mrp)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Mrp);
                                else if (item.OrderBy.Price)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.Price);
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.PublishedOn);
                                else if (item.OrderBy.TotalViews)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.ProductDetailView.TotalViews);
                                else if (item.OrderBy.LastViewedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.ProductDetailView.LastViewedOn);
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new ProductDetail
            {
                Id = x.Id,
                Title = x.Title,
                ProductDetailLink = x.ProductDetailLink,
                ShopCategoryId = x.ShopCategoryId,
                IsAvailable = x.IsAvailable,
                ExchangePolicyId = x.ExchangePolicyId,
                DeliveryPolicyId = x.DeliveryPolicyId,
                ReturnPolicyId = x.ReturnPolicyId,
                Stock = x.Stock,
                Mrp = x.Mrp,
                Price = x.Price,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                PublishedById = x.PublishedById,
                ProductDetailWorkerId = x.ProductDetailWorkerId,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                ProductDetailView = x.ProductDetailView
            }).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(ProductDetailDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        public Task<List<ProductDetail>> GetForListViewAsync(ProductDetailDataTableForViewRequest requestModel, List<int> downloadableDeliveryPolicyIds)
        {
            var queryable = dbContext.ProductDetails.AsQueryable();

            if (!string.IsNullOrEmpty(requestModel.Search))
                queryable = queryable.Where(x => x.Title.Contains(requestModel.Search)
                || x.Keyword1.Contains(requestModel.Search)
                || x.Keyword2.Contains(requestModel.Search)
                || x.Keyword3.Contains(requestModel.Search)
                || x.Keyword4.Contains(requestModel.Search)
                || x.Keyword5.Contains(requestModel.Search));

            if (requestModel.ShopCategoryId.HasValue)
                queryable = queryable.Where(x => x.ShopCategoryId == requestModel.ShopCategoryId.Value);

            queryable = queryable.Where(x => x.IsPublished && x.IsAvailable && (x.Stock > 0 || downloadableDeliveryPolicyIds.Contains(x.DeliveryPolicyId))).OrderByDescending(x => x.PublishedOn);

            return queryable.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new ProductDetail
            {
                Id = x.Id,
                Title = x.Title,
                MainImage = x.MainImage,
                ShopCategoryId = x.ShopCategoryId,
                ProductDetailLink = x.ProductDetailLink,
                Mrp = x.Mrp,
                Price = x.Price,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                UpdatedOn = x.UpdatedOn,
                IsAvailable = x.IsAvailable,
                ExchangePolicyId = x.ExchangePolicyId,
                DeliveryPolicyId = x.DeliveryPolicyId,
                ReturnPolicyId = x.ReturnPolicyId
            }).AsNoTracking().ToListAsync();
        }

        private IQueryable<ProductDetail> CommonSearch(ProductDetailDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.ProductDetails.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Title.Contains(requestModel.Search.Value)
                || x.ProductDetailLink == requestModel.Search.Value);

            if (!string.IsNullOrEmpty(requestModel.ProductDetailWorkerId))
                queriableEntity = queriableEntity.Where(x => x.ProductDetailWorkerId == requestModel.ProductDetailWorkerId);

            if (requestModel.LastViewedOnStart.HasValue)
                queriableEntity = queriableEntity.Where(x => x.ProductDetailView.LastViewedOn >= requestModel.LastViewedOnStart.Value);

            if (requestModel.LastViewedOnEnd.HasValue)
                queriableEntity = queriableEntity.Where(x => x.ProductDetailView.LastViewedOn < requestModel.LastViewedOnEnd.Value);

            return queriableEntity;
        }

        public Task<ProductDetail> GetForViewAsync(int shopCategoryId, string productdetailLink, List<int> downloadableDeliveryPolicyIds)
        {
            return dbContext.ProductDetails
                .Where(pd => pd.ShopCategoryId == shopCategoryId
                          && pd.ProductDetailLink == productdetailLink
                          && pd.IsPublished
                          && pd.IsAvailable
                          && (pd.Stock > 0 || downloadableDeliveryPolicyIds.Contains(pd.DeliveryPolicyId)))
                .Select(pd => new ProductDetail
                {
                    Id = pd.Id,
                    Title = pd.Title,
                    ProductDetailLink = pd.ProductDetailLink,
                    MainImage = pd.MainImage,
                    MetaDescription = pd.MetaDescription,
                    ShopCategoryId = pd.ShopCategoryId,
                    ExchangePolicyId = pd.ExchangePolicyId,
                    DeliveryPolicyId = pd.DeliveryPolicyId,
                    ReturnPolicyId = pd.ReturnPolicyId,
                    Description = pd.Description,
                    Keyword1 = pd.Keyword1,
                    Keyword2 = pd.Keyword2,
                    Keyword3 = pd.Keyword3,
                    Keyword4 = pd.Keyword4,
                    Keyword5 = pd.Keyword5,
                    Mrp = pd.Mrp,
                    Price = pd.Price,
                    IsPublished = pd.IsPublished,
                    PublishedOn = pd.PublishedOn,
                    UpdatedOn = pd.UpdatedOn,
                    IsAvailable = pd.IsAvailable,
                    PublishedById = pd.PublishedById,

                    ProductVariant = pd.ProductVariant == null ? null : new()
                    {
                        Product = pd.ProductVariant.Product == null ? null : new()
                        {
                            ProductVariants = pd.ProductVariant.Product.ProductVariants
                                .Where(pv => pv.ProductDetail.IsPublished
                                          && pv.ProductDetail.IsAvailable
                                          && (pv.ProductDetail.Stock > 0
                                              || downloadableDeliveryPolicyIds.Contains(pv.ProductDetail.DeliveryPolicyId)))
                                .Select(pv => new ProductVariant
                                {
                                    ProductDetail = new()
                                    {
                                        Id = pv.ProductDetail.Id,
                                        Title = pv.ProductDetail.Title,
                                        MainImage = pv.ProductDetail.MainImage,
                                        ShopCategoryId = pv.ProductDetail.ShopCategoryId,
                                        ProductDetailLink = pv.ProductDetail.ProductDetailLink,
                                        Mrp = pv.ProductDetail.Mrp,
                                        Price = pv.ProductDetail.Price,
                                        IsPublished = pv.ProductDetail.IsPublished,
                                        PublishedOn = pv.ProductDetail.PublishedOn,
                                        UpdatedOn = pv.ProductDetail.UpdatedOn,
                                        IsAvailable = pv.ProductDetail.IsAvailable,
                                        ExchangePolicyId = pv.ProductDetail.ExchangePolicyId,
                                        DeliveryPolicyId = pv.ProductDetail.DeliveryPolicyId,
                                        ReturnPolicyId = pv.ProductDetail.ReturnPolicyId
                                    }
                                }).ToList()
                        }
                    }
                }).AsNoTracking().SingleOrDefaultAsync();
        }

        public Task<List<ProductDetailView>> GetProductDetailViewWithLockByProductDetailIdsAsync(IEnumerable<int> productdetailIds)
        {
            return dbContext.ProductDetailViews.FromSqlRaw("select * from ProductDetailViews with(rowlock, updlock, holdlock)")
                            .Where(x => productdetailIds.Contains(x.Id))
                            .ToListAsync();
        }

        public void AddProductDetailView(ProductDetailView entity)
        {
            dbContext.ProductDetailViews.Add(entity);
        }

        public Task<int> GetTotalUniquePagesVisitedAsync(DateTimeOffset? lastViewedOnStart = null, DateTimeOffset? lastViewedOnEnd = null)
        {
            var query = dbContext.ProductDetailViews.AsQueryable();

            if (lastViewedOnStart.HasValue)
                query = query.Where(x => x.LastViewedOn >= lastViewedOnStart.Value.UtcDateTime);

            if (lastViewedOnEnd.HasValue)
                query = query.Where(x => x.LastViewedOn < lastViewedOnEnd.Value.UtcDateTime);

            return query.AsNoTracking().CountAsync();
        }

        public Task<int> GetTotalPagesVisitedLifetimeAsync()
        {
            return dbContext.ProductDetailViews.AsNoTracking().SumAsync(x => x.TotalViews);
        }
    }
}
