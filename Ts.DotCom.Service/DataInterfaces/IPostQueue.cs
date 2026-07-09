namespace Ts.DotCom.Service.DataInterfaces
{
    [Obsolete($"This interface is deprecated. Use {nameof(IPostViewCounter)} instead.")]
    public interface IPostQueue
    {
        void Enqueue(int id);
        IEnumerable<int> DequeueAll();
        bool IsEmpty { get; }
    }
}
