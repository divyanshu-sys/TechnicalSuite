using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ProductFluent : BaseEntityFluent<Product>
    {
        public ProductFluent(EntityTypeBuilder<Product> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Title).IsUnique(true);

            builder.Property(x => x.Title).HasColumnType("nvarchar").HasMaxLength(120).IsRequired(true);
            builder.Property(x => x.MainImage).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.IsAvailable).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.PublishedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.ProductWorkerId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);

            // Navigation references start
            // Navigation references end
        }
    }
}
