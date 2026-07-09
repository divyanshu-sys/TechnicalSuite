using Newtonsoft.Json;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailImageVms
{
    public class ProductDetailImageVm
    {
        public int Id { get; set; }
        public string ImageNames { get; set; }
        public IEnumerable<ProductDetailImageHelperVm> ProductDetailImageHelpers => JsonConvert.DeserializeObject<IEnumerable<ProductDetailImageHelperVm>>(ImageNames);
        public string ImageBaseUrl { get; set; }
    }

    public class ProductDetailImageHelperVm
    {
        public string Name { get; set; }
    }
}
