using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class CurrencyTypeFluent
    {
        public CurrencyTypeFluent(EntityTypeBuilder<CurrencyType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Symbol).IsUnique(true);
            builder.HasIndex(x => x.Letter).IsUnique(true);

            builder.Property(x => x.Symbol).HasColumnType("nvarchar").HasMaxLength(5).IsRequired(true);
            builder.Property(x => x.Letter).HasColumnType("nvarchar").HasMaxLength(3).IsFixedLength(true).IsRequired(true);

            // Navigation references start
            // Navigation references end
        }
    }
}
