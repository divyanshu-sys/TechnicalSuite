namespace Ts.Dto
{
    public class DataTableResponseDto<TEntity> where TEntity : class
    {
        public DataTableResponseDto()
        {
            AaData = new();
        }

        public List<TEntity> AaData { get; set; }
        public long RecordsTotal { get; set; }
        public long RecordsFiltered { get; set; }
    }
}
