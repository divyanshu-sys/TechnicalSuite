using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class BlogImageFluent : BaseEntityFluent<BlogImage>
    {
        public BlogImageFluent(EntityTypeBuilder<BlogImage> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("BlogId");

            builder.Property(x => x.ImageNames).HasColumnType("nvarchar(max)").IsRequired(true);

            // Navigation references start
            builder.HasOne(x => x.Blog).WithOne(x => x.BlogImage)
                .HasForeignKey<BlogImage>(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            // Navigation references end
        }
    }
}
