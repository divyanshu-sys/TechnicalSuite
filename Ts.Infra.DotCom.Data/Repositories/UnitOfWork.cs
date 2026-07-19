using Ts.DotCom.Domain.Interfaces;
using Ts.Infra.DotCom.Data.Context;
namespace Ts.Infra.DotCom.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPostRepository _postRepository;
        private IStoryRepository _storyRepository;
        private bool disposedValue;

        public IPostRepository PostRepo => _postRepository ??= new PostRepository(dbContext);
        public IStoryRepository StoryRepo => _storyRepository ??= new StoryRepository(dbContext);

        private readonly DotComDbContext dbContext;

        public UnitOfWork(DotComDbContext dbContext)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }

                _postRepository = null;
                _storyRepository = null;

                disposedValue = true;
            }
        }

        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
