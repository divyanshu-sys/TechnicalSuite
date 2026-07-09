using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class ApplicationUserFluent
    {
        public ApplicationUserFluent(EntityTypeBuilder<ApplicationUser> builder)
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
            builder.Property(x => x.CreatedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.CreatedOn).HasColumnType("datetime2").HasMaxLength(7).IsRequired(true);
            builder.Property(x => x.UpdatedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime2").HasMaxLength(7);
            builder.Property(x => x.IpAddress).HasColumnType("varchar").HasMaxLength(16);
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.CanLogin).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.ChangePassword).IsRequired(true);
            builder.Property(x => x.DateOfBirth).HasColumnType("date");

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedApplicationUsers)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedApplicationUsers)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Gender).WithMany(x => x.ApplicationUsers)
                .HasForeignKey(x => x.GenderId).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end

            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
