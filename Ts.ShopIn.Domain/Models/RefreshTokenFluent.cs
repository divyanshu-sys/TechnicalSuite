using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class RefreshTokenFluent
    {
        public RefreshTokenFluent(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(250);

            builder.HasIndex(x => x.UserId).IsUnique(true);

            builder.Property(x => x.RefreshReloginId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.UserId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.User).WithOne(x => x.CreatedRefreshToken)
                .HasForeignKey<RefreshToken>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
