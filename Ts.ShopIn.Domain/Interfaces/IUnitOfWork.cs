namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBlogRepository BlogRepo { get; }
        ICartRepository CartRepo { get; }
        IClientUserRepository ClientUserRepo { get; }
        IRefreshTokenRepository RefreshTokenRepo { get; }
        IOrderDetailRepository OrderDetailRepo { get; }
        IOrderDetailStatusHistoryRepository OrderDetailStatusHistoryRepo { get; }
        IOrderRepository OrderRepo { get; }
        IPaymentRepository PaymentRepo { get; }
        IPaymentStatusHistoryRepository PaymentStatusHistoryRepo { get; }
        IProductDetailRepository ProductDetailRepo { get; }
        IProductDetailDocumentRepository ProductDetailDocumentRepo { get; }
        IProductRepository ProductRepo { get; }
        ILoginLogRepository LoginLogRepo { get; }
        INextUserSettingRepository NextUserSettingRepo { get; }
        INextOrderSettingRepository NextOrderSettingRepo { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
