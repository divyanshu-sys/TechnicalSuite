using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ts.Client.HttpClientServices;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.ProductVms;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class ProductForViewClient : IProductForViewClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly ILogger<ProductForViewClient> logger;
        private readonly string BaseUrl;

        public ProductForViewClient(IHttpPublicService httpPublicService, IConfiguration configuration,
            ILogger<ProductForViewClient> logger)
        {
            this.httpPublicService = httpPublicService;
            this.logger = logger;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<IEnumerable<GetProductForListViewVm>>> GetForListViewAsync(ProductDataTableForViewRequestDto model)
        {
            var response = await httpPublicService.PostAsync<IEnumerable<GetProductForListViewVm>>($"{BaseUrl}/{ApiUrl}/productdotin/product-list-for-view", model, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error fetching product list for view: {ErrorMessages}", string.Join(',', response.ErrorMessage));
            return response;
        }
    }
}
