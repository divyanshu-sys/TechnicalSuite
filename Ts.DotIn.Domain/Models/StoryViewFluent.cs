using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotIn.Domain.Models
{
    public class StoryViewFluent
    {
        public StoryViewFluent(EntityTypeBuilder<StoryView> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("StoryId");

            builder.HasIndex(x => x.LastViewedOn);

            builder.Property(x => x.TotalViews).IsRequired(true);
            builder.Property(x => x.LastViewedOn).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Story).WithOne(x => x.StoryView)
                .HasForeignKey<StoryView>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
