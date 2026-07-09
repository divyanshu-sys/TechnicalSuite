namespace Ts.DotIn.Domain.DataTableModels.PostDataTables
{
    public class PostDataTableRequest : BaseDataTableRequest<PostOrder>
    {
        public string PostWorkerId { get; set; }
        public DateTime? LastViewedOnStart { get; set; }
        public DateTime? LastViewedOnEnd { get; set; }
    }
}
