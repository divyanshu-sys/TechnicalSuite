using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductVariantFluent
    {
        public ProductVariantFluent(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(x => x.ProductDetailId);

            builder.HasIndex(x => x.ProductId);

            // Navigation references start
            builder.HasOne(x => x.Product).WithMany(p => p.ProductVariants)
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProductDetail).WithOne(pd => pd.ProductVariant)
               .HasForeignKey<ProductVariant>(x => x.ProductDetailId)
               .OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
