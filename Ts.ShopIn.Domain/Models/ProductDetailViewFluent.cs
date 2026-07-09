using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailViewFluent
    {
        public ProductDetailViewFluent(EntityTypeBuilder<ProductDetailView> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ProductDetailId");

            builder.HasIndex(x => x.LastViewedOn);

            builder.Property(x => x.TotalViews).IsRequired(true);
            builder.Property(x => x.LastViewedOn).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.ProductDetail).WithOne(x => x.ProductDetailView)
                .HasForeignKey<ProductDetailView>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
