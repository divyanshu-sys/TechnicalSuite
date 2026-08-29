using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.DataTableModels.ProductDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ShopInDbContext dbContext;

        public ProductRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        override public Task<List<Product>> GetAllAsync()
        {
            return dbContext.Products.OrderByDescending(x => x.CreatedOn).AsNoTracking().ToListAsync();
        }

        public Task<List<Product>> GetAllAsync(ProductDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<Product> orderQueriableEntity = null;
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
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.PublishedOn);
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
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.PublishedOn);
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
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.PublishedOn);
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
                                else if (item.OrderBy.PublishedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.PublishedOn);
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Product
            {
                Id = x.Id,
                Title = x.Title,
                IsAvailable = x.IsAvailable,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                PublishedById = x.PublishedById,
                ProductWorkerId = x.ProductWorkerId,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn
            }).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(ProductDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        public Task<List<Product>> GetForListViewAsync(ProductDataTableForViewRequest requestModel, List<int> downloadableDeliveryPolicyIds)
        {
            var queryable = dbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(requestModel.Search))
                queryable = queryable.Where(x => x.Title.Contains(requestModel.Search));

            queryable = queryable
                .Where(x => x.IsPublished && x.IsAvailable)
                .Where(x => x.ProductVariants.Any(pv => pv.ProductDetail.IsPublished && pv.ProductDetail.IsAvailable
                && (pv.ProductDetail.Stock > 0 || downloadableDeliveryPolicyIds.Contains(pv.ProductDetail.DeliveryPolicyId))))
                .OrderByDescending(x => x.PublishedOn)
                .Skip(requestModel.Start)
                .Take(requestModel.Length);

            return queryable.Select(x => new Product
            {
                Id = x.Id,
                Title = x.Title,
                MainImage = x.MainImage,
                IsPublished = x.IsPublished,
                PublishedOn = x.PublishedOn,
                UpdatedOn = x.UpdatedOn,
                IsAvailable = x.IsAvailable,
                ProductVariants = x.ProductVariants
                    .Where(pv => pv.ProductDetail.IsPublished && pv.ProductDetail.IsAvailable
                    && (pv.ProductDetail.Stock > 0 || downloadableDeliveryPolicyIds.Contains(pv.ProductDetail.DeliveryPolicyId)))
                    .Take(1)
                    .Select(pv => new ProductVariant
                    {
                        ProductDetail = new ProductDetail
                        {
                            Id = pv.ProductDetailId,
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
                    })
                    .ToList()
            }).AsNoTracking().ToListAsync();
        }

        private IQueryable<Product> CommonSearch(ProductDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Products.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.Title.Contains(requestModel.Search.Value));

            if (!string.IsNullOrEmpty(requestModel.ProductWorkerId))
                queriableEntity = queriableEntity.Where(x => x.ProductWorkerId == requestModel.ProductWorkerId);

            return queriableEntity;
        }
    }
}
