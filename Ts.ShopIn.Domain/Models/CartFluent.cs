using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class CartFluent : ClientBaseEntityFluent<Cart>
    {
        public CartFluent(EntityTypeBuilder<Cart> builder) : base(builder)
        {
            builder.HasKey(x => new { x.ProductDetailId, x.UserId });

            builder.Property(x => x.ProductDetailId).IsRequired(true);
            builder.Property(x => x.ItemCount).IsRequired(true);
            builder.Property(x => x.UserId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.ProductDetail).WithMany(x => x.Carts)
                .HasForeignKey(x => x.ProductDetailId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.CartAddedBy).WithMany(x => x.CartAddedByUsers)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
