using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class LoginLogFluent
    {
        public LoginLogFluent(EntityTypeBuilder<LoginLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(250);

            builder.Property(x => x.LoggedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);
            builder.Property(x => x.UserId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.IpAddress).HasColumnType("varchar").HasMaxLength(16).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.LoggedInBy).WithMany(x => x.CreatedLoginLogs)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
