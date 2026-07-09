using Microsoft.EntityFrameworkCore;
using Ts.Common.Constant.AppConstants;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.HelperModels;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ShopInDbContext dbContext;

        public OrderDetailRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<OrderDetail> GetByAsync(int id, string userId, params int[] orderStatusIds)
        {
            return dbContext.OrderDetails.Where(x => x.Id == id && x.Order.UserId == userId && orderStatusIds.Contains(x.OrderStatusId))
                .Include(x => x.ProductDetail).ThenInclude(x => x.ProductDetailDocument)
                .SingleOrDefaultAsync();
        }

        public Task<OrderDetail> GetLatestFirstByProductDetailIdAsync(int productDetailId, int orderStatusId)
        {
            return dbContext.OrderDetails.Where(x => x.ProductDetailId == productDetailId && x.OrderStatusId == orderStatusId)
                .OrderByDescending(x => x.UpdatedOn)
                .FirstOrDefaultAsync();
        }

        public Task<decimal> GetTotalPriceAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, params int[] orderStatusIds)
        {
            var query = dbContext.OrderDetails.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(x => x.UpdatedOn >= startDate.Value.UtcDateTime);

            if (endDate.HasValue)
                query = query.Where(x => x.UpdatedOn < endDate.Value.UtcDateTime);

            if (orderStatusIds?.Length > 0)
                query = query.Where(x => orderStatusIds.Contains(x.OrderStatusId));

            return query.SumAsync(x => x.TotalPrice);
        }

        public Task<List<TotalPriceByMonthsHelperModel>> GetTotalPriceByMonthsAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, params int[] orderStatusIds)
        {
            var query = dbContext.OrderDetails.Where(x => x.UpdatedOn != null).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(x => x.UpdatedOn >= startDate.Value.UtcDateTime);

            if (endDate.HasValue)
                query = query.Where(x => x.UpdatedOn < endDate.Value.UtcDateTime);

            if (orderStatusIds?.Length > 0)
                query = query.Where(x => orderStatusIds.Contains(x.OrderStatusId));

            var orderQuery = query
                .GroupBy(x => new
                {
                    x.UpdatedOn.Value.Month
                })
                .Select(g => new
                {
                    g.Key.Month,
                    TotalPrice = g.Sum(x => x.TotalPrice)
                })
                .OrderBy(x => x.Month);

            return orderQuery.Select(x => new TotalPriceByMonthsHelperModel
            {
                Month = TimeZoneInfoConstant.MonthNames[x.Month - 1],
                TotalPrice = x.TotalPrice
            }).ToListAsync();
        }
    }
}
