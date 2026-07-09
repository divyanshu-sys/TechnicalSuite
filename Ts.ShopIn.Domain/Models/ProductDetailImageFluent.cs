using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailImageFluent : BaseEntityFluent<ProductDetailImage>
    {
        public ProductDetailImageFluent(EntityTypeBuilder<ProductDetailImage> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ProductId");

            builder.Property(x => x.ImageNames).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.ProductDetail).WithOne(x => x.ProductDetailImage)
                .HasForeignKey<ProductDetailImage>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
