namespace Ts.Domain.DataTableModels
{
    public abstract class BaseDataTableRequest<TOrder> where TOrder : class
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public Search Search { get; set; }
        public IEnumerable<SortOrder<TOrder>> OrderList { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class SortOrder<TOrder>
    {
        public TOrder OrderBy { get; set; }
        public bool IsAsc { get; set; }
    }
}
