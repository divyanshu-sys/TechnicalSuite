using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class PaymentFluent
    {
        public PaymentFluent(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("OrderId");

            builder.HasIndex(x => x.CreatedOn);
            builder.HasIndex(x => x.UpdatedOn);

            builder.Property(x => x.CurrencyTypeId).IsRequired(true);
            builder.Property(x => x.Amount).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.Razorpay_Method).HasColumnType("varchar").HasMaxLength(150);
            builder.Property(x => x.Razorpay_Payment_Id).HasColumnType("varchar").HasMaxLength(450);
            builder.Property(x => x.Razorpay_Order_Id).HasColumnType("varchar").HasMaxLength(450);
            builder.Property(x => x.Razorpay_Amount_Refunded).HasColumnType("decimal(18,3)").IsRequired(true);
            builder.Property(x => x.Razorpay_Refund_Status).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.Razorpay_Created_At).HasColumnType("bigint");
            builder.Property(x => x.PaymentGatewayTypeId).IsRequired(true);
            builder.Property(x => x.CreatedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime2").HasMaxLength(7);

            // Navigation references start
            builder.HasOne(x => x.Order).WithOne(x => x.Payment)
                .HasForeignKey<Payment>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
