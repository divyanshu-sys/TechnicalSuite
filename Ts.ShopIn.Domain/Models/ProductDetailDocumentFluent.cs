using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductDetailDocumentFluent : BaseEntityFluent<ProductDetailDocument>
    {
        public ProductDetailDocumentFluent(EntityTypeBuilder<ProductDetailDocument> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ProductDetailId");

            builder.Property(x => x.Document).HasColumnType("varchar(450)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.ProductDetail).WithOne(x => x.ProductDetailDocument)
                .HasForeignKey<ProductDetailDocument>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
