using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class ClientUserFluent
    {
        public ClientUserFluent(EntityTypeBuilder<ClientUser> builder)
        {
            builder.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(250);

            builder.HasIndex(x => x.ProfileImageName).IsUnique(true);
            builder.HasIndex(x => new { x.FirstName, x.LastName });
            builder.HasIndex(x => x.CreatedOn);
            builder.HasIndex(x => x.UpdatedOn);

            builder.Property(x => x.FirstName).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.LastName).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.PhoneCode).HasColumnType("varchar").HasMaxLength(5);
            builder.Property(x => x.ProfileImageName).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.CreatedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime2").HasMaxLength(7);
            builder.Property(x => x.IpAddress).HasColumnType("varchar").HasMaxLength(16);
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.CanLogin).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.DateOfBirth).HasColumnType("date");

            // Navigation references start
            // Navigation references end
            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
