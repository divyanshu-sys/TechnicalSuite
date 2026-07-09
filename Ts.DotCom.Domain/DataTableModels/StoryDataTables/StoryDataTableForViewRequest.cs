namespace Ts.DotCom.Domain.DataTableModels.StoryDataTables
{
    public class StoryDataTableForViewRequest
    {
        public string Search { get; set; }
        public int? SubCategoryId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
