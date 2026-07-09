namespace Ts.DotCom.Domain.DataTableModels.StoryDataTables
{
    public class StoryDataTableRequest : BaseDataTableRequest<StoryOrder>
    {
        public string StoryWorkerId { get; set; }
    }
}
