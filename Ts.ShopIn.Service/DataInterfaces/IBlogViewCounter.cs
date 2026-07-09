namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IBlogViewCounter
    {
        Task IncrementAsync(int blogId);
        Dictionary<int, int> SnapshotAndReset();
    }
}
