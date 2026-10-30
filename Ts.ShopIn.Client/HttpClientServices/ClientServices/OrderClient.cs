using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.OrderVms;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.HomeDtos;

namespace Ts.ShopIn.Client.HttpClientServices.ClientServices
{
    public class OrderClient : IOrderClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly string BaseUrl;

        public OrderClient(IHttpClientService httpClientService, IConfiguration configuration, IMapper mapper)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<OrderVm>>> GetAllAsync(OrderDataTableRequestVm model)
        {
            var orderModel = mapper.Map<OrderDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<OrderOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 1)
                        sort.OrderBy.OrderedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    orderModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<OrderVm>>($"{BaseUrl}/{ApiUrl}/orderdotin/datatable", orderModel, true);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
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
                ;
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
                ;
        }
    }
}
