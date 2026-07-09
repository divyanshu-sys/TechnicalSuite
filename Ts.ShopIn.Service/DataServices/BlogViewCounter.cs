using System.Collections.Concurrent;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.ShopIn.Service.DataServices
{
    public class BlogViewCounter : IBlogViewCounter
    {
        private ConcurrentDictionary<int, int> _views = new();

        public Task IncrementAsync(int blogId)
        {
            _views.AddOrUpdate(blogId, 1, (key, oldValue) => oldValue + 1);
            return Task.CompletedTask;
        }

        public Dictionary<int, int> SnapshotAndReset()
        {
            var oldDict = Interlocked.Exchange(
                ref _views,
                new ConcurrentDictionary<int, int>()
            );

            return new Dictionary<int, int>(oldDict);
        }
    }
}
