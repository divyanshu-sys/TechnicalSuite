using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductImageFluent : BaseEntityFluent<ProductImage>
    {
        public ProductImageFluent(EntityTypeBuilder<ProductImage> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ProductId");

            builder.Property(x => x.ImageNames).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Product).WithOne(x => x.ProductImage)
                .HasForeignKey<ProductImage>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
