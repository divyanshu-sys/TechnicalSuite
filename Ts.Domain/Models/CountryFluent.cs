using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class CountryFluent : BaseEntityFluent<Country>
    {
        public CountryFluent(EntityTypeBuilder<Country> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Name).IsUnique(true);
            builder.HasIndex(x => x.Code2).IsUnique(true);
            builder.HasIndex(x => x.Code3).IsUnique(true);

            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Code2).HasColumnType("varchar").HasMaxLength(2).IsFixedLength(true).IsRequired(true);
            builder.Property(x => x.Code3).HasColumnType("varchar").HasMaxLength(3).IsFixedLength(true).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedCountries)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedCountries)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end
        }
    }
}
