using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.DotCom.Domain.Models
{
    public class StoryFluent : BaseEntityFluent<Story>
    {
        public StoryFluent(EntityTypeBuilder<Story> builder) : base(builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Keyword1);
            builder.HasIndex(x => x.Keyword2);
            builder.HasIndex(x => x.Keyword3);
            builder.HasIndex(x => x.Keyword4);
            builder.HasIndex(x => x.Keyword5);
            builder.HasIndex(x => x.StoryLink).IsUnique(true);

            builder.Property(x => x.Title).HasColumnType("nvarchar").HasMaxLength(70).IsRequired(true);
            builder.Property(x => x.StoryLink).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.MainImage).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.MainImageSource).HasColumnType("nvarchar").HasMaxLength(250);
            builder.Property(x => x.MainDescription).HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(x => x.MetaDescription).HasColumnType("nvarchar").HasMaxLength(160).IsRequired(true);
            builder.Property(x => x.SubCategoryId).IsRequired(true);
            builder.Property(x => x.Keyword1).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Keyword2).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Keyword3).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Keyword4).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Keyword5).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.PublishedById).HasColumnType("varchar").HasMaxLength(250);
            builder.Property(x => x.StoryWorkerId).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.StorySource).HasColumnType("nvarchar").HasMaxLength(250);

            // Navigation references start
            // Navigation references end
        }
    }
}
