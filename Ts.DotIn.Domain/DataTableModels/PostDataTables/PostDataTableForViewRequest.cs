namespace Ts.DotIn.Domain.DataTableModels.PostDataTables
{
    public class PostDataTableForViewRequest
    {
        public string Search { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
