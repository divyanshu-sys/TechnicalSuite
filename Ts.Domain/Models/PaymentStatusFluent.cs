using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class PaymentStatusFluent
    {
        public PaymentStatusFluent(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Name).IsUnique(true);

            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.PaymentGatewayType).WithMany(x => x.PaymentStatuses)
                .HasForeignKey(x => x.PaymentGatewayTypeId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
