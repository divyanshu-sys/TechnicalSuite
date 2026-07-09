namespace Ts.DotIn.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepo { get; }
        IStoryRepository StoryRepo { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
