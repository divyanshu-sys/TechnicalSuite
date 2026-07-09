using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.ShopCategoryVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.ShopCategoryDataTableDtos;
using Ts.Dto.ShopCategoryDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class ShopCategoryClient : IShopCategoryClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public ShopCategoryClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<ShopCategoryVm>>> GetAllAsync(ShopCategoryDataTableRequestVm model)
        {
            var postModel = mapper.Map<ShopCategoryDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<ShopCategoryOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 0)
                        sort.OrderBy.Name = true;
                    else if (order.Column == 1)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 2)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<ShopCategoryVm>>($"{BaseUrl}/{ApiUrl}/shopcategory/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<IEnumerable<ShopCategoryVm>>> GetAllAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<ShopCategoryVm>>($"{BaseUrl}/{ApiUrl}/shopcategory", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync()
        {
            var vms = await GetAllAsync().ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }

        public async Task<ResponseMessageDto<ShopCategoryVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<ShopCategoryVm>($"{BaseUrl}/{ApiUrl}/shopcategory/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateShopCategoryVm>> GetForEditAsync(int id)
        {
            var vm = await GetAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateShopCategoryVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<ShopCategoryVm>> PostAsync(CreateShopCategoryVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateShopCategoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<ShopCategoryVm>($"{BaseUrl}/{ApiUrl}/shopcategory", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateShopCategoryVm model, int shopCategoryId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateShopCategoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/shopcategory/{shopCategoryId}", modelDto, true).ConfigureAwait(false);
        }
    }
}
