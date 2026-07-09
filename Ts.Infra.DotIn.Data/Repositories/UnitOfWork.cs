using Ts.DotIn.Domain.Interfaces;
using Ts.Infra.DotIn.Data.Context;
namespace Ts.Infra.DotIn.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPostRepository _postRepository;
        private IStoryRepository _storyRepository;

        public IPostRepository PostRepo => _postRepository ??= new PostRepository(dbContext);
        public IStoryRepository StoryRepo => _storyRepository ??= new StoryRepository(dbContext);

        private readonly DotInDbContext dbContext;

        public UnitOfWork(DotInDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public Task BeginTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync();
        }

        public Task CommitAsync()
        {
            return dbContext.Database.CurrentTransaction == null ? Task.CompletedTask : dbContext.Database.CurrentTransaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return dbContext.Database.CurrentTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
