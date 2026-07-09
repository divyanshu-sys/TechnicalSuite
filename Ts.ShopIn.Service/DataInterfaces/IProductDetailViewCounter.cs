namespace Ts.ShopIn.Service.DataInterfaces
{
    public interface IProductDetailViewCounter
    {
        Task IncrementAsync(int productDetailId);
        Dictionary<int, int> SnapshotAndReset();
    }
}
