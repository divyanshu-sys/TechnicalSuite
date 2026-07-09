using System.ComponentModel.DataAnnotations;
namespace Ts.Dto.DataTableDtos
{
    public abstract class BaseDataTableRequestDto<TOrder> where TOrder : class
    {
        protected BaseDataTableRequestDto()
        {
            OrderList = [];
        }

        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }

        public SearchDto Search { get; set; }

        public List<SortOrderDto<TOrder>> OrderList { get; set; }
    }

    public class SearchDto
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class SortOrderDto<TOrder> where TOrder : class
    {
        public TOrder OrderBy { get; set; }
        public bool IsAsc { get; set; }
    }
}
