using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotCom.Domain.Models
{
    public class PostFluent : BaseEntityFluent<Post>
    {
        public PostFluent(EntityTypeBuilder<Post> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Keyword1);
            builder.HasIndex(x => x.Keyword2);
            builder.HasIndex(x => x.Keyword3);
            builder.HasIndex(x => x.Keyword4);
            builder.HasIndex(x => x.Keyword5);
            builder.HasIndex(x => x.PostLink).IsUnique(true);

            builder.Property(x => x.Title).HasColumnType("nvarchar").HasMaxLength(80).IsRequired(true);
            builder.Property(x => x.PostLink).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.MainImage).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.MetaDescription).HasColumnType("nvarchar").HasMaxLength(180).IsRequired(true);
            builder.Property(x => x.CategoryId).IsRequired(true);
            builder.Property(x => x.SubCategoryId).IsRequired(true);
            builder.Property(x => x.Keyword1).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Keyword2).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Keyword3).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Keyword4).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Keyword5).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.PublishedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.PostWorkerId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.MainImageSource).HasColumnType("nvarchar").HasMaxLength(250);
            builder.Property(x => x.PostSource).HasColumnType("nvarchar").HasMaxLength(250);

            // Navigation references start
            // Navigation references end
        }
    }
}
