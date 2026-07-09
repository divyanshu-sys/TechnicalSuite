using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class OrderDetailStatusHistoryFluent
    {
        public OrderDetailStatusHistoryFluent(EntityTypeBuilder<OrderDetailStatusHistory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("OrderDetailId");

            builder.Property(x => x.OrderStatuses).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.OrderDetail).WithOne(x => x.OrderDetailStatusHistories)
                .HasForeignKey<OrderDetailStatusHistory>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
