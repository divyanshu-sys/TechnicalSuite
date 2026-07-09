using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class AddressFluent : BaseEntityFluent<Address>
    {
        public AddressFluent(EntityTypeBuilder<Address> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.UserId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Address1).HasColumnType("varchar").HasMaxLength(450).IsRequired(true);
            builder.Property(x => x.Address2).HasColumnType("varchar").HasMaxLength(450);
            builder.Property(x => x.PostOfficeId).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedAddresses)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedAddresses)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ApplicationUser).WithMany(x => x.Addresses)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.PostOffice).WithMany(x => x.Addresses)
                .HasForeignKey(x => x.PostOfficeId).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end
        }
    }
}
