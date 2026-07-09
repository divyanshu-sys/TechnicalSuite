using Newtonsoft.Json;
namespace Ts.DotIn.Client.ViewModels.StoryImageVms
{
    public class StoryImageVm
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public IEnumerable<StoryImageHelperVm> StoryImageHelpers => JsonConvert.DeserializeObject<IEnumerable<StoryImageHelperVm>>(ImageNames);
        public string ImageBaseUrl { get; set; }
    }

    public class StoryImageHelperVm
    {
        public string Name { get; set; }
        public string Source { get; set; }
    }
}
