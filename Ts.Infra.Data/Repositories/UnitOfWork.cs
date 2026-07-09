using Ts.Domain.Interfaces;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPostOfficeRepository _postOfficeRepository;
        private IDistrictRepository _districtRepository;
        private IStateRepository _stateRepository;
        private ICountryRepository _countryRepository;
        private INextUserSettingRepository _nextUserSettingRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private ILoginLogRepository _loginLogRepository;
        private ICategoryRepository _categoryRepository;
        private ISubCategoryRepository _subCategoryRepository;
        private IShopCategoryRepository _shopCategoryRepository;
        private IExchangePolicyRepository _exchangePolicyRepository;
        private IDeliveryPolicyRepository _deliveryPolicyRepository;
        private IReturnPolicyRepository _returnPolicyRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private IPaymentModeRepository _paymentModeRepository;
        private IOrderStatusRepository _orderStatusRepository;
        private IPaymentGatewayTypeRepository _paymentGatewayTypeRepository;
        private IPaymentStatusRepository _paymentStatusRepository;
        private ICurrencyTypeRepository _currencyTypeRepository;

        public IPostOfficeRepository PostOfficeRepo => _postOfficeRepository ??= new PostOfficeRepository(dbContext);
        public IDistrictRepository DistrictRepo => _districtRepository ??= new DistrictRepository(dbContext);
        public IStateRepository StateRepo => _stateRepository ??= new StateRepository(dbContext);
        public ICountryRepository CountryRepo => _countryRepository ??= new CountryRepository(dbContext);
        public INextUserSettingRepository NextUserSettingRepo => _nextUserSettingRepository ??= new NextUserSettingRepository(dbContext);
        public IRefreshTokenRepository RefreshTokenRepo => _refreshTokenRepository ??= new RefreshTokenRepository(dbContext);
        public ILoginLogRepository LoginLogRepo => _loginLogRepository ??= new LoginLogRepository(dbContext);
        public ICategoryRepository CategoryRepo => _categoryRepository ??= new CategoryRepository(dbContext);
        public ISubCategoryRepository SubCategoryRepo => _subCategoryRepository ??= new SubCategoryRepository(dbContext);
        public IShopCategoryRepository ShopCategoryRepo => _shopCategoryRepository ??= new ShopCategoryRepository(dbContext);
        public IExchangePolicyRepository ExchangePolicyRepo => _exchangePolicyRepository ??= new ExchangePolicyRepository(dbContext);
        public IDeliveryPolicyRepository DeliveryPolicyRepo => _deliveryPolicyRepository ??= new DeliveryPolicyRepository(dbContext);
        public IReturnPolicyRepository ReturnPolicyRepo => _returnPolicyRepository ??= new ReturnPolicyRepository(dbContext);
        public IPaymentModeRepository PaymentModeRepo => _paymentModeRepository ??= new PaymentModeRepository(dbContext);
        public IOrderStatusRepository OrderStatusRepo => _orderStatusRepository ??= new OrderStatusRepository(dbContext);
        public IApplicationUserRepository ApplicationUserRepo => _applicationUserRepository ??= new ApplicationUserRepository(dbContext);
        public IPaymentGatewayTypeRepository PaymentGatewayTypeRepo => _paymentGatewayTypeRepository ??= new PaymentGatewayTypeRepository(dbContext);
        public IPaymentStatusRepository PaymentStatusRepo => _paymentStatusRepository ??= new PaymentStatusRepository(dbContext);
        public ICurrencyTypeRepository CurrencyTypeRepo => _currencyTypeRepository ??= new CurrencyTypeRepository(dbContext);

        private readonly TsIdentityDbContext dbContext;

        public UnitOfWork(TsIdentityDbContext dbContext)
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
