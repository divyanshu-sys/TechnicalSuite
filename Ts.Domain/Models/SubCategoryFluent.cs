using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.Domain.Models
{
    public class SubCategoryFluent : BaseEntityFluent<SubCategory>
    {
        public SubCategoryFluent(EntityTypeBuilder<SubCategory> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Name).IsUnique(true);

            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedSubCategories)
                .HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.UpdatedBy).WithMany(x => x.UpdatedSubCategories)
                .HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end
        }
    }
}
