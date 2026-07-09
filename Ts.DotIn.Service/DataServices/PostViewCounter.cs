using System.Collections.Concurrent;
using Ts.DotIn.Service.DataInterfaces;
namespace Ts.DotIn.Service.DataServices
{
    public class PostViewCounter : IPostViewCounter
    {
        private ConcurrentDictionary<int, int> _views = new();

        public Task IncrementAsync(int postId)
        {
            _views.AddOrUpdate(postId, 1, (key, oldValue) => oldValue + 1);
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
