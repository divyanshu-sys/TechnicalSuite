using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotIn.Domain.Models
{
    public class PostRelativeFluent : BaseEntityFluent<PostRelative>
    {
        public PostRelativeFluent(EntityTypeBuilder<PostRelative> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PostId");

            builder.Property(x => x.RelativeUrl).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Post).WithOne(x => x.PostRelative)
                .HasForeignKey<PostRelative>(x => x.Id).OnDelete(DeleteBehavior.NoAction);
            // Navigation references end
        }
    }
}
