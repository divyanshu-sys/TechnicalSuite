using Newtonsoft.Json;
namespace Ts.DotCom.Client.ViewModels.PostImageVms
{
    public class PostImageVm
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public IEnumerable<PostImageHelperVm> PostImageHelpers => JsonConvert.DeserializeObject<IEnumerable<PostImageHelperVm>>(ImageNames);
        public string ImageBaseUrl { get; set; }
    }

    public class PostImageHelperVm
    {
        public string Name { get; set; }
        public string Source { get; set; }
    }
}
