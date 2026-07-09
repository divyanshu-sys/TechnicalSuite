using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotCom.Domain.Models
{
    public abstract class BaseEntityFluent<TEntity> where TEntity : BaseEntity
    {
        protected BaseEntityFluent(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(x => x.CreatedOn);
            builder.HasIndex(x => x.UpdatedOn);

            builder.Property(x => x.CreatedById).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.CreatedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);
            builder.Property(x => x.UpdatedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime2").HasMaxLength(7);
            builder.Property(x => x.IpAddress).HasColumnType("varchar").HasMaxLength(16).IsRequired(true);
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.ConcurrencyTimestamp).HasColumnType("timestamp").ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("current_timestamp").IsConcurrencyToken(true);

            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
