using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ts.Domain.Models;
using Ts.Infra.Data.Extensions;
namespace Ts.Infra.Data.Context
{
    public class TsIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public TsIdentityDbContext(DbContextOptions<TsIdentityDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<NextUserSetting> NextUserSettings { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PostOffice> PostOffices { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ShopCategory> ShopCategories { get; set; }
        public DbSet<ExchangePolicy> ExchangePolicies { get; set; }
        public DbSet<DeliveryPolicy> DeliveryPolicies { get; set; }
        public DbSet<ReturnPolicy> ReturnPolicies { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentGatewayType> PaymentGatewayTypes { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("dbo");

            _ = new ApplicationUserFluent(builder.Entity<ApplicationUser>());
            _ = new ApplicationRoleFluent(builder.Entity<ApplicationRole>());
            _ = new CountryFluent(builder.Entity<Country>());
            _ = new GenderFluent(builder.Entity<Gender>());
            _ = new NextUserSettingFluent(builder.Entity<NextUserSetting>());
            _ = new RefreshTokenFluent(builder.Entity<RefreshToken>());
            _ = new LoginLogFluent(builder.Entity<LoginLog>());
            _ = new AddressFluent(builder.Entity<Address>());
            _ = new DistrictFluent(builder.Entity<District>());
            _ = new PostOfficeFluent(builder.Entity<PostOffice>());
            _ = new StateFluent(builder.Entity<State>());
            _ = new CategoryFluent(builder.Entity<Category>());
            _ = new SubCategoryFluent(builder.Entity<SubCategory>());
            _ = new ShopCategoryFluent(builder.Entity<ShopCategory>());
            _ = new ExchangePolicyFluent(builder.Entity<ExchangePolicy>());
            _ = new DeliveryPolicyFluent(builder.Entity<DeliveryPolicy>());
            _ = new ReturnPolicyFluent(builder.Entity<ReturnPolicy>());
            _ = new PaymentModeFluent(builder.Entity<PaymentMode>());
            _ = new OrderStatusFluent(builder.Entity<OrderStatus>());
            _ = new PaymentGatewayTypeFluent(builder.Entity<PaymentGatewayType>());
            _ = new CurrencyTypeFluent(builder.Entity<CurrencyType>());
            _ = new PaymentStatusFluent(builder.Entity<PaymentStatus>());

            builder.SeedData();
            base.OnModelCreating(builder);
        }
    }
}
