using Microsoft.EntityFrameworkCore;
using Ts.DotIn.Domain.Models;
namespace Ts.Infra.DotIn.Data.Context
{
    public class DotInDbContext : DbContext
    {
        public DotInDbContext(DbContextOptions<DotInDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostView> PostViews { get; set; }
        public DbSet<PostRelative> PostRelatives { get; set; }

        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryImage> StoryImages { get; set; }
        public DbSet<StoryView> StoryViews { get; set; }
        public DbSet<StoryRelative> StoryRelatives { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("dbo");

            _ = new PostFluent(builder.Entity<Post>());
            _ = new PostImageFluent(builder.Entity<PostImage>());
            _ = new PostViewFluent(builder.Entity<PostView>());
            _ = new PostRelativeFluent(builder.Entity<PostRelative>());

            _ = new StoryFluent(builder.Entity<Story>());
            _ = new StoryImageFluent(builder.Entity<StoryImage>());
            _ = new StoryViewFluent(builder.Entity<StoryView>());
            _ = new StoryRelativeFluent(builder.Entity<StoryRelative>());

            base.OnModelCreating(builder);
        }
    }
}
