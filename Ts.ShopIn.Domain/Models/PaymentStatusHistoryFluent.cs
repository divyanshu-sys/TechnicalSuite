using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class PaymentStatusHistoryFluent
    {
        public PaymentStatusHistoryFluent(EntityTypeBuilder<PaymentStatusHistory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PaymentId");

            builder.Property(x => x.PaymentStatuses).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Payment).WithOne(x => x.PaymentStatusHistory)
                .HasForeignKey<PaymentStatusHistory>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
