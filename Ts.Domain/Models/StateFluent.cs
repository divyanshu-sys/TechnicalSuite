using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class StateFluent : BaseEntityFluent<State>
    {
        public StateFluent(EntityTypeBuilder<State> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Name).IsUnique(true);
            builder.HasIndex(x => x.Code2).IsUnique(true);
            builder.HasIndex(x => x.CountryId);

            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Code2).HasColumnType("varchar").HasMaxLength(2).IsFixedLength(true).IsRequired(true);
            builder.Property(x => x.CountryId).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedStates)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedStates)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Country).WithMany(x => x.States)
                .HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
