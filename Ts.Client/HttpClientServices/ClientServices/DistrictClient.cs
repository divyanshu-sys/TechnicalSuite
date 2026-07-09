using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.DistrictVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.DistrictDataTableDtos;
using Ts.Dto.DistrictDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class DistrictClient : IDistrictClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public DistrictClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/district/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<DistrictVm>>> GetAllAsync(DistrictDataTableRequestVm model)
        {
            var postModel = mapper.Map<DistrictDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<DistrictOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 0)
                        sort.OrderBy.Name = true;
                    else if (order.Column == 3)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 4)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<DistrictVm>>($"{BaseUrl}/{ApiUrl}/district/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<IEnumerable<DistrictVm>>> GetAllByStateIdAsync(int stateId)
        {
            return await httpClientService.GetAsync<IEnumerable<DistrictVm>>($"{BaseUrl}/{ApiUrl}/district/by-stateid/{stateId}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownByStateIdAsync(int stateId)
        {
            var vms = await GetAllByStateIdAsync(stateId).ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }

        public async Task<ResponseMessageDto<DistrictVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<DistrictVm>($"{BaseUrl}/{ApiUrl}/district/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateDistrictVm>> GetForEditAsync(int id)
        {
            var vm = await httpClientService.GetAsync<DistrictVm>($"{BaseUrl}/{ApiUrl}/district/{id}/for-edit", true).ConfigureAwait(false);

            var updateVm = mapper.Map<ResponseMessageDto<UpdateDistrictVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<DistrictVm>> PostAsync(CreateDistrictVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateDistrictDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<DistrictVm>($"{BaseUrl}/{ApiUrl}/district", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateDistrictVm model, int districtId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateDistrictDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/district/{districtId}", modelDto, true).ConfigureAwait(false);
        }
    }
}
