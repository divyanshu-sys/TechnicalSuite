using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class OrderDetailFluent
    {
        public OrderDetailFluent(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.CreatedOn);
            builder.HasIndex(x => x.UpdatedOn);

            builder.Property(x => x.OrderId).IsRequired(true);
            builder.Property(x => x.OrderStatusId).IsRequired(true);
            builder.Property(x => x.Title).HasColumnType("nvarchar").HasMaxLength(120).IsRequired(true);
            builder.Property(x => x.ItemCount).IsRequired(true);
            builder.Property(x => x.CurrencyTypeId).IsRequired(true);
            builder.Property(x => x.ItemPrice).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.ShopCategoryId).IsRequired(true);
            builder.Property(x => x.ExchangePolicyId).IsRequired(true);
            builder.Property(x => x.DeliveryPolicyId).IsRequired(true);
            builder.Property(x => x.ReturnPolicyId).IsRequired(true);
            builder.Property(x => x.CreatedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime2").HasMaxLength(7);

            // Navigation references start
            builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.ProductDetail).WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.ProductDetailId).OnDelete(DeleteBehavior.SetNull);
            // Navigation references end
        }
    }
}
