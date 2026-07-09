using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotIn.Domain.Models
{
    public class PostImageFluent : BaseEntityFluent<PostImage>
    {
        public PostImageFluent(EntityTypeBuilder<PostImage> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PostId");

            builder.Property(x => x.ImageNames).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Post).WithOne(x => x.PostImage)
                .HasForeignKey<PostImage>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
