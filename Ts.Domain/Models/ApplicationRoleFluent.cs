using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class ApplicationRoleFluent
    {
        public ApplicationRoleFluent(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired(true);

            // Navigation references start
            // Navigation references end

            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
