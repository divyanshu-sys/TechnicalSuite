using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class DeliveryPolicyFluent
    {
        public DeliveryPolicyFluent(EntityTypeBuilder<DeliveryPolicy> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Name).IsUnique(true);

            builder.Property(x => x.DeliveryInDays).IsRequired(true);
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Title).HasColumnType("varchar").HasMaxLength(300).IsRequired(true);

            // Navigation references start
            // Navigation references end
        }
    }
}
