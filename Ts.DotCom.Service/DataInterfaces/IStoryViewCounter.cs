namespace Ts.DotCom.Service.DataInterfaces
{
    public interface IStoryViewCounter
    {
        Task IncrementAsync(int storyId);
        Dictionary<int, int> SnapshotAndReset();
    }
}
