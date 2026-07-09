using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Extensions;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Context
{
    public class ShopInDbContext : IdentityDbContext<ClientUser>
    {
        public ShopInDbContext(DbContextOptions<ShopInDbContext> options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<BlogView> BlogViews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<NextOrderSetting> NextOrderSettings { get; set; }
        public DbSet<NextUserSetting> NextUserSettings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailStatusHistory> OrderDetailStatusHistories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentStatusHistory> PaymentStatusHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductDetailImage> ProductDetailImages { get; set; }
        public DbSet<ProductDetailView> ProductDetailViews { get; set; }
        public DbSet<ProductDetailDocument> ProductDetailDocuments { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("dbo");

            _ = new BlogFluent(builder.Entity<Blog>());
            _ = new BlogImageFluent(builder.Entity<BlogImage>());
            _ = new BlogViewFluent(builder.Entity<BlogView>());
            _ = new CartFluent(builder.Entity<Cart>());
            _ = new RefreshTokenFluent(builder.Entity<RefreshToken>());
            _ = new ClientUserFluent(builder.Entity<ClientUser>());
            _ = new NextOrderSettingFluent(builder.Entity<NextOrderSetting>());
            _ = new NextUserSettingFluent(builder.Entity<NextUserSetting>());
            _ = new OrderFluent(builder.Entity<Order>());
            _ = new OrderDetailFluent(builder.Entity<OrderDetail>());
            _ = new OrderDetailStatusHistoryFluent(builder.Entity<OrderDetailStatusHistory>());
            _ = new PaymentFluent(builder.Entity<Payment>());
            _ = new PaymentStatusHistoryFluent(builder.Entity<PaymentStatusHistory>());
            _ = new ProductFluent(builder.Entity<Product>());
            _ = new ProductImageFluent(builder.Entity<ProductImage>());
            _ = new ProductDetailFluent(builder.Entity<ProductDetail>());
            _ = new ProductDetailImageFluent(builder.Entity<ProductDetailImage>());
            _ = new ProductDetailViewFluent(builder.Entity<ProductDetailView>());
            _ = new ProductDetailDocumentFluent(builder.Entity<ProductDetailDocument>());
            _ = new ProductVariantFluent(builder.Entity<ProductVariant>());
            _ = new LoginLogFluent(builder.Entity<LoginLog>());

            builder.SeedData();
            base.OnModelCreating(builder);
        }
    }
}
