using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> GetRazorpayPaymentWithLockByAsync(string razorpayPaymentId = null, string razorpayOrderId = null, string userId = null);
    }
}
