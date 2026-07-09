using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class BlogViewFluent
    {
        public BlogViewFluent(EntityTypeBuilder<BlogView> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("BlogId");

            builder.HasIndex(x => x.LastViewedOn);

            builder.Property(x => x.TotalViews).IsRequired(true);
            builder.Property(x => x.LastViewedOn).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Blog).WithOne(x => x.BlogView)
                .HasForeignKey<BlogView>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
