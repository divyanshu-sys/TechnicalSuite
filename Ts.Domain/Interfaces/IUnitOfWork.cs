namespace Ts.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostOfficeRepository PostOfficeRepo { get; }
        IDistrictRepository DistrictRepo { get; }
        IStateRepository StateRepo { get; }
        ICountryRepository CountryRepo { get; }
        INextUserSettingRepository NextUserSettingRepo { get; }
        IRefreshTokenRepository RefreshTokenRepo { get; }
        ILoginLogRepository LoginLogRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        ISubCategoryRepository SubCategoryRepo { get; }
        IShopCategoryRepository ShopCategoryRepo { get; }
        IExchangePolicyRepository ExchangePolicyRepo { get; }
        IDeliveryPolicyRepository DeliveryPolicyRepo { get; }
        IReturnPolicyRepository ReturnPolicyRepo { get; }
        IPaymentModeRepository PaymentModeRepo { get; }
        IOrderStatusRepository OrderStatusRepo { get; }
        IApplicationUserRepository ApplicationUserRepo { get; }
        IPaymentGatewayTypeRepository PaymentGatewayTypeRepo { get; }
        IPaymentStatusRepository PaymentStatusRepo { get; }
        ICurrencyTypeRepository CurrencyTypeRepo { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
