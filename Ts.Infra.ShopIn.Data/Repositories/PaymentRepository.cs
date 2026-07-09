using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ShopInDbContext dbContext;

        public PaymentRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Payment> GetRazorpayPaymentWithLockByAsync(string razorpayPaymentId = null, string razorpayOrderId = null, string userId = null)
        {
            var sql = @"SELECT p.* FROM Payments p WITH (ROWLOCK, UPDLOCK, HOLDLOCK)
                INNER JOIN Orders o WITH (ROWLOCK, UPDLOCK, HOLDLOCK) ON p.OrderId = o.Id";

            var conditions = new List<string>();
            var parameters = new List<object>();

            if (!string.IsNullOrEmpty(razorpayPaymentId))
            {
                conditions.Add($"p.Razorpay_Payment_Id = {{{parameters.Count}}}");
                parameters.Add(razorpayPaymentId);
            }

            if (!string.IsNullOrEmpty(razorpayOrderId))
            {
                conditions.Add($"p.Razorpay_Order_Id = {{{parameters.Count}}}");
                parameters.Add(razorpayOrderId);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                conditions.Add($"o.UserId = {{{parameters.Count}}}");
                parameters.Add(userId);
            }

            if (conditions.Count == 0)
                throw new ArgumentException("At least one filter must be provided.");

            var finalSql = $"{sql} WHERE {string.Join(" AND ", conditions)}";

            return dbContext.Payments
                .FromSqlRaw(finalSql, parameters.ToArray())
                .AsSplitQuery()
                .Include(x => x.PaymentStatusHistory)
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderedBy)
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderDetails)
                        .ThenInclude(x => x.OrderDetailStatusHistories)
                .SingleOrDefaultAsync();
        }
    }
}
