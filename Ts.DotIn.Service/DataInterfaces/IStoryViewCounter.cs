namespace Ts.DotIn.Service.DataInterfaces
{
    public interface IStoryViewCounter
    {
        Task IncrementAsync(int storyId);
        Dictionary<int, int> SnapshotAndReset();
    }
}
