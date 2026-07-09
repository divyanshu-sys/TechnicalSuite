using Newtonsoft.Json;
namespace Ts.ShopIn.Client.ViewModels.ProductImageVms
{
    public class ProductImageVm
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public IEnumerable<ProductImageHelperVm> ProductImageHelpers => JsonConvert.DeserializeObject<IEnumerable<ProductImageHelperVm>>(ImageNames);
        public string ImageBaseUrl { get; set; }
    }

    public class ProductImageHelperVm
    {
        public string Name { get; set; }
    }
}
