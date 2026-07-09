using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class OrderFluent
    {
        public OrderFluent(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OrderNumber);
            builder.HasIndex(x => x.OrderedOn);

            builder.Property(x => x.PaymentModeId).IsRequired(true);
            builder.Property(x => x.TotalItemCount).IsRequired(true);
            builder.Property(x => x.CurrencyTypeId).IsRequired(true);
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.OrderNumber).HasColumnType("varchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.ClientName).HasColumnType("varchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.UserId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.OrderedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.OrderedBy).WithMany(x => x.OrderAddedByUsers)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end
        }
    }
}
