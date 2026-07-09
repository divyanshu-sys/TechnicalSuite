using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailFluent : BaseEntityFluent<ProductDetail>
    {
        public ProductDetailFluent(EntityTypeBuilder<ProductDetail> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Keyword1);
            builder.HasIndex(x => x.Keyword2);
            builder.HasIndex(x => x.Keyword3);
            builder.HasIndex(x => x.Keyword4);
            builder.HasIndex(x => x.Keyword5);
            builder.HasIndex(x => x.ProductDetailLink).IsUnique(true);

            builder.Property(x => x.Title).HasColumnType("nvarchar").HasMaxLength(120).IsRequired(true);
            builder.Property(x => x.ProductDetailLink).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.MainImage).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.MetaDescription).HasColumnType("nvarchar").HasMaxLength(180).IsRequired(true);
            builder.Property(x => x.ShopCategoryId).IsRequired(true);
            builder.Property(x => x.ExchangePolicyId).IsRequired(true);
            builder.Property(x => x.DeliveryPolicyId).IsRequired(true);
            builder.Property(x => x.ReturnPolicyId).IsRequired(true);
            builder.Property(x => x.Stock).IsRequired(true);
            builder.Property(x => x.Mrp).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.Price).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.Keyword1).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Keyword2).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Keyword3).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Keyword4).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Keyword5).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.IsAvailable).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.PublishedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.ProductDetailWorkerId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);

            // Navigation references start
            // Navigation references end
        }
    }
}
