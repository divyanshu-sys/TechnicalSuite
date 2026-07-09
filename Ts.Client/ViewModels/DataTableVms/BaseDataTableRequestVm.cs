using System.ComponentModel.DataAnnotations;
namespace Ts.Client.ViewModels.DataTableVms
{
    public abstract class BaseDataTableRequestVm
    {
        [Range(0, int.MaxValue)]
        public int Draw { get; set; }

        [Range(0, int.MaxValue)]
        public int Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Length { get; set; }

        public SearchVm Search { get; set; }

        public IEnumerable<SortOrderVm> Order { get; set; }
    }

    public class SearchVm
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class SortOrderVm
    {
        [Range(0, int.MaxValue)]
        public int Column { get; set; }

        [Required]
        public string Dir { get; set; }
    }
}
