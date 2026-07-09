using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.Interfaces;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IBlogRepository _blogRepository;
        private ICartRepository _cartRepository;
        private IClientUserRepository _clientUserRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IOrderDetailStatusHistoryRepository _orderDetailStatusHistoryRepository;
        private IOrderRepository _orderRepository;
        private IPaymentRepository _paymentRepository;
        private IPaymentStatusHistoryRepository _paymentStatusHistoryRepository;
        private IProductDetailRepository _productDetailRepository;
        private IProductDetailDocumentRepository _productDetailDocumentRepository;
        private IProductRepository _productRepository;
        private ILoginLogRepository _loginLogRepository;
        private INextUserSettingRepository _nextUserSettingRepository;
        private INextOrderSettingRepository _nextOrderSettingRepository;

        public IBlogRepository BlogRepo => _blogRepository ??= new BlogRepository(dbContext);
        public ICartRepository CartRepo => _cartRepository ??= new CartRepository(dbContext);
        public IClientUserRepository ClientUserRepo => _clientUserRepository ??= new ClientUserRepository(dbContext);
        public IRefreshTokenRepository RefreshTokenRepo => _refreshTokenRepository ??= new RefreshTokenRepository(dbContext);
        public IOrderDetailRepository OrderDetailRepo => _orderDetailRepository ??= new OrderDetailRepository(dbContext);
        public IOrderDetailStatusHistoryRepository OrderDetailStatusHistoryRepo => _orderDetailStatusHistoryRepository ??= new OrderDetailStatusHistoryRepository(dbContext);
        public IOrderRepository OrderRepo => _orderRepository ??= new OrderRepository(dbContext);
        public IPaymentRepository PaymentRepo => _paymentRepository ??= new PaymentRepository(dbContext);
        public IPaymentStatusHistoryRepository PaymentStatusHistoryRepo => _paymentStatusHistoryRepository ??= new PaymentStatusHistoryRepository(dbContext);
        public IProductDetailRepository ProductDetailRepo => _productDetailRepository ??= new ProductDetailRepository(dbContext);
        public IProductDetailDocumentRepository ProductDetailDocumentRepo => _productDetailDocumentRepository ??= new ProductDetailDocumentRepository(dbContext);
        public IProductRepository ProductRepo => _productRepository ??= new ProductRepository(dbContext);
        public ILoginLogRepository LoginLogRepo => _loginLogRepository ??= new LoginLogRepository(dbContext);
        public INextUserSettingRepository NextUserSettingRepo => _nextUserSettingRepository ??= new NextUserSettingRepository(dbContext);
        public INextOrderSettingRepository NextOrderSettingRepo => _nextOrderSettingRepository ??= new NextOrderSettingRepository(dbContext);

        private readonly ShopInDbContext dbContext;

        public UnitOfWork(ShopInDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public Task BeginTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync();
        }

        public Task CommitAsync()
        {
            return dbContext.Database.CurrentTransaction == null ? Task.CompletedTask : dbContext.Database.CurrentTransaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return dbContext.Database.CurrentTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
