namespace Ts.Client.ViewModels.DataTableVms
{
    public class DataTableResponseVm<TEntity> where TEntity : class
    {
        public DataTableResponseVm()
        {
            AaData = new();
        }

        public List<TEntity> AaData { get; set; }
        public long RecordsTotal { get; set; }
        public long RecordsFiltered { get; set; }
        public int Draw { get; set; }
    }
}
