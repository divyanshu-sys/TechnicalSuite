using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.DataTableModels.OrderDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ShopInDbContext dbContext;

        public OrderRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Order>> GetAllAsync(OrderDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<Order> orderQueriableEntity = null;
            if (requestModel.OrderList.Any())
            {
                foreach (var item in requestModel.OrderList)
                {
                    if (item.OrderBy != null)
                        if (item.IsAsc)
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.OrderedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.OrderedOn);
                            }
                            else
                            {
                                if (item.OrderBy.OrderedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.OrderedOn);
                            }
                        }
                        else
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.OrderedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.OrderedOn);
                            }
                            else
                            {
                                if (item.OrderBy.OrderedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.OrderedOn);
                            }
                        }
                }
            }
            if (orderQueriableEntity != null)
                queriableEntity = orderQueriableEntity.AsQueryable();

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new Order
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                ClientName = x.ClientName,
                PaymentModeId = x.PaymentModeId,
                TotalItemCount = x.TotalItemCount,
                CurrencyTypeId = x.CurrencyTypeId,
                TotalAmount = x.TotalAmount,
                UserId = x.UserId,
                OrderedOn = x.OrderedOn,
                Payment = x.Payment == null ? null : new()
                {
                    Id = x.Payment.Id,
                    PaymentGatewayTypeId = x.Payment.PaymentGatewayTypeId,
                    PaymentStatusId = x.Payment.PaymentStatusId
                }
            }).AsNoTracking().ToListAsync();
        }

        public Task<List<Order>> GetForListViewAsync(OrderDataTableForViewRequest requestModel, string userId, int orderStatusIdFilterOut)
        {
            return dbContext.Orders
            .Where(x => x.UserId == userId && x.OrderDetails.Any(od => od.OrderStatusId != orderStatusIdFilterOut))
            .Select(x => new Order
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                TotalItemCount = x.TotalItemCount,
                CurrencyTypeId = x.CurrencyTypeId,
                TotalAmount = x.TotalAmount,
                OrderedOn = x.OrderedOn,
                OrderDetails = x.OrderDetails.Where(od => od.OrderStatusId != orderStatusIdFilterOut)
                    .Select(x => new OrderDetail
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        OrderStatusId = x.OrderStatusId,
                        ProductDetailId = x.ProductDetailId,
                        Title = x.Title,
                        ItemCount = x.ItemCount,
                        CurrencyTypeId = x.CurrencyTypeId,
                        ItemPrice = x.ItemPrice,
                        TotalPrice = x.TotalPrice,
                        ExchangePolicyId = x.ExchangePolicyId,
                        DeliveryPolicyId = x.DeliveryPolicyId,
                        ReturnPolicyId = x.ReturnPolicyId,
                        UpdatedOn = x.UpdatedOn,
                        ProductDetail = x.ProductDetail == null ? null : new()
                        {
                            Id = x.ProductDetail.Id,
                            Title = x.ProductDetail.Title,
                            ProductDetailLink = x.ProductDetail.ProductDetailLink,
                            MainImage = x.ProductDetail.MainImage
                        }
                    }).ToList()
            })
            .OrderByDescending(x => x.OrderedOn)
            .Skip(requestModel.Start)
            .Take(requestModel.Length)
            .AsNoTracking()
            .ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(OrderDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        private IQueryable<Order> CommonSearch(OrderDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Orders.AsQueryable();

            // Global Search
            if (!string.IsNullOrWhiteSpace(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.OrderNumber.Contains(requestModel.Search.Value));

            return queriableEntity;
        }
    }
}
