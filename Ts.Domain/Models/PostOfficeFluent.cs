using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class PostOfficeFluent : BaseEntityFluent<PostOffice>
    {
        public PostOfficeFluent(EntityTypeBuilder<PostOffice> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Name).IsUnique(true);
            builder.HasIndex(x => x.DistrictId);

            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Pincode).HasColumnType("varchar").HasMaxLength(6).IsFixedLength(true).IsRequired(true);
            builder.Property(x => x.DistrictId).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedPostOffices)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedPostOffices)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.District).WithMany(x => x.PostOffices)
                .HasForeignKey(x => x.DistrictId).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
