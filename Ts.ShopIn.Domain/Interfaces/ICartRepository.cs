using Ts.ShopIn.Domain.Models;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetAsync(int productDetailId, string userId);
        Task<List<Cart>> GetByUserIdAsync(string userId);
        Task<int> DeleteByUserIdAsync(string userId);
    }
}
