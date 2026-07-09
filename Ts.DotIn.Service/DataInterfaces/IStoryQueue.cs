namespace Ts.DotIn.Service.DataInterfaces
{
    [Obsolete($"This interface is deprecated. Use {nameof(IStoryViewCounter)} instead.")]
    public interface IStoryQueue
    {
        void Enqueue(int id);
        IEnumerable<int> DequeueAll();
        bool IsEmpty { get; }
    }
}
