using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices;
using Ts.Dto;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Dto.HomeDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class OrderClient : IOrderClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly string BaseUrl;

        public OrderClient(IHttpClientService httpClientService, IConfiguration configuration)
        {
            this.httpClientService = httpClientService;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<ChartResponseDto>> GetSalesData(string startDate = null, string endDate = null)
        {
            var baseUrl = $"{BaseUrl}/{ApiUrl}/orderdotin/total-sales-data";

            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(startDate))
                queryParams["startDate"] = startDate;

            if (!string.IsNullOrWhiteSpace(endDate))
                queryParams["endDate"] = endDate;

            var finalUrl = queryParams.Count > 0
                ? QueryHelpers.AddQueryString(baseUrl, queryParams)
                : baseUrl;

            return await httpClientService
                .GetAsync<ChartResponseDto>(finalUrl, true)
                .ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<decimal>> GetTotalRevenue(string startDate = null, string endDate = null)
        {
            var baseUrl = $"{BaseUrl}/{ApiUrl}/orderdotin/total-revenue";

            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(startDate))
                queryParams["startDate"] = startDate;

            if (!string.IsNullOrWhiteSpace(endDate))
                queryParams["endDate"] = endDate;

            var finalUrl = queryParams.Count > 0
                ? QueryHelpers.AddQueryString(baseUrl, queryParams)
                : baseUrl;

            return await httpClientService
                .GetAsync<decimal>(finalUrl, true)
                .ConfigureAwait(false);
        }
    }
}
