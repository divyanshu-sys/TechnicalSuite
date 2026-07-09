using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class DistrictFluent : BaseEntityFluent<District>
    {
        public DistrictFluent(EntityTypeBuilder<District> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.StateId);
            builder.HasIndex(x => x.Name).IsUnique(true);

            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.StateId).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedDistricts)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedDistricts)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.State).WithMany(x => x.Districts)
                .HasForeignKey(x => x.StateId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
