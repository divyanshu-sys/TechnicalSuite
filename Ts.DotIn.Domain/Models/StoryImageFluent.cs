using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotIn.Domain.Models
{
    public class StoryImageFluent : BaseEntityFluent<StoryImage>
    {
        public StoryImageFluent(EntityTypeBuilder<StoryImage> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("StoryId");

            builder.Property(x => x.ImageNames).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Story).WithOne(x => x.StoryImage)
                .HasForeignKey<StoryImage>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
