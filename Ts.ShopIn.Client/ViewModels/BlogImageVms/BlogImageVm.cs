using Newtonsoft.Json;
namespace Ts.ShopIn.Client.ViewModels.BlogImageVms
{
    public class BlogImageVm
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public IEnumerable<BlogImageHelperVm> BlogImageHelpers => JsonConvert.DeserializeObject<IEnumerable<BlogImageHelperVm>>(ImageNames);
        public string ImageBaseUrl { get; set; }
    }

    public class BlogImageHelperVm
    {
        public string Name { get; set; }
        public string Source { get; set; }
    }
}
