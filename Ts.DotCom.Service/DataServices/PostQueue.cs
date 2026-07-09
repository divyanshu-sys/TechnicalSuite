using System.Collections.Concurrent;
using Ts.DotCom.Service.DataInterfaces;
namespace Ts.DotCom.Service.DataServices
{
    [Obsolete($"This class is deprecated and will be removed in future versions. Please use the {nameof(PostViewCounter)} instead.")]
    public class PostQueue : IPostQueue
    {
        private readonly ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();

        public void Enqueue(int id)
        {
            _queue.Enqueue(id);
        }

        public IEnumerable<int> DequeueAll()
        {
            var items = new List<int>();
            while (_queue.TryDequeue(out var id))
            {
                items.Add(id);
            }
            return items;
        }

        public bool IsEmpty => _queue.IsEmpty;
    }
}
