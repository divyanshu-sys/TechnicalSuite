namespace Ts.DotCom.Service.DataInterfaces
{
    public interface IPostViewCounter
    {
        Task IncrementAsync(int postId);
        Dictionary<int, int> SnapshotAndReset();
    }
}
