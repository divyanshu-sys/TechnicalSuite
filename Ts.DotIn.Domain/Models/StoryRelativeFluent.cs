using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotIn.Domain.Models
{
    public class StoryRelativeFluent : BaseEntityFluent<StoryRelative>
    {
        public StoryRelativeFluent(EntityTypeBuilder<StoryRelative> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("StoryId");

            builder.Property(x => x.RelativeUrl).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Story).WithOne(x => x.StoryRelative)
                .HasForeignKey<StoryRelative>(x => x.Id).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end
        }
    }
}
