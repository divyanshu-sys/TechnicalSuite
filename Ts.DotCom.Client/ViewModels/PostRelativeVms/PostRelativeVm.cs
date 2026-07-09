using Newtonsoft.Json;
namespace Ts.DotCom.Client.ViewModels.PostRelativeVms
{
    public class PostRelativeVm
    {
        public int Id { get; set; }
        public string RelativeUrl { get; set; }
        public IEnumerable<PostRelativeHelperVm> PostRelativeHelpers => JsonConvert.DeserializeObject<IEnumerable<PostRelativeHelperVm>>(RelativeUrl);
    }

    public class PostRelativeHelperVm
    {
        public string HrefLang { get; set; }
        public string Href { get; set; }
    }
}
