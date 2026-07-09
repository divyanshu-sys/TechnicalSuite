using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class ProductDetailDocumentRepository : GenericRepository<ProductDetailDocument>, IProductDetailDocumentRepository
    {
        private readonly ShopInDbContext dbContext;

        public ProductDetailDocumentRepository(ShopInDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
