namespace Ts.DotIn.Domain.DataTableModels.StoryDataTables
{
    public class StoryDataTableRequest : BaseDataTableRequest<StoryOrder>
    {
        public string StoryWorkerId { get; set; }
    }
}
