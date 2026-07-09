using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotCom.Domain.Models
{
    public class PostViewFluent
    {
        public PostViewFluent(EntityTypeBuilder<PostView> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.LastViewedOn);

            builder.Property(x => x.Id).HasColumnName("PostId");
            builder.Property(x => x.TotalViews).IsRequired(true);
            builder.Property(x => x.LastViewedOn).IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Post).WithOne(x => x.PostView)
                .HasForeignKey<PostView>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
