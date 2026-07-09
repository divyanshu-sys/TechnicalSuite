using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly ShopInDbContext dbContext;

        public CartRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<int> DeleteByUserIdAsync(string userId)
        {
            return dbContext.Carts.Where(c => c.UserId == userId).ExecuteDeleteAsync();
        }

        public Task<Cart> GetAsync(int productDetailId, string userId)
        {
            return dbContext.Carts.SingleOrDefaultAsync(c => c.ProductDetailId == productDetailId && c.UserId == userId);
        }

        public Task<List<Cart>> GetByUserIdAsync(string userId)
        {
            return dbContext.Carts.Where(c => c.UserId == userId).OrderByDescending(c => c.CreatedOn)
                .Select(c => new Cart()
                {
                    ProductDetailId = c.ProductDetailId,
                    ItemCount = c.ItemCount,
                    UserId = c.UserId,
                    ProductDetail = new()
                    {
                        Id = c.ProductDetail.Id,
                        Title = c.ProductDetail.Title,
                        MainImage = c.ProductDetail.MainImage,
                        ShopCategoryId = c.ProductDetail.ShopCategoryId,
                        ProductDetailLink = c.ProductDetail.ProductDetailLink,
                        Mrp = c.ProductDetail.Mrp,
                        Price = c.ProductDetail.Price,
                        IsPublished = c.ProductDetail.IsPublished,
                        PublishedOn = c.ProductDetail.PublishedOn,
                        UpdatedOn = c.ProductDetail.UpdatedOn,
                        IsAvailable = c.ProductDetail.IsAvailable,
                        ExchangePolicyId = c.ProductDetail.ExchangePolicyId,
                        DeliveryPolicyId = c.ProductDetail.DeliveryPolicyId,
                        ReturnPolicyId = c.ProductDetail.ReturnPolicyId
                    }
                }).ToListAsync();
        }
    }
}
