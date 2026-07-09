using Newtonsoft.Json;
namespace Ts.DotCom.Client.ViewModels.StoryRelativeVms
{
    public class StoryRelativeVm
    {
        public int Id { get; set; }
        public string RelativeUrl { get; set; }
        public IEnumerable<StoryRelativeHelperVm> StoryRelativeHelpers => JsonConvert.DeserializeObject<IEnumerable<StoryRelativeHelperVm>>(RelativeUrl);
    }

    public class StoryRelativeHelperVm
    {
        public string HrefLang { get; set; }
        public string Href { get; set; }
    }
}
