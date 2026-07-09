using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.StateVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.StateDataTableDtos;
using Ts.Dto.StateDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class StateClient : IStateClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public StateClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/state/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<StateVm>>> GetAllAsync(StateDataTableRequestVm model)
        {
            var postModel = mapper.Map<StateDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<StateOrderDto>
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
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<StateVm>>($"{BaseUrl}/{ApiUrl}/state/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<IEnumerable<StateVm>>> GetAllByCountryIdAsync(int countryId)
        {
            return await httpClientService.GetAsync<IEnumerable<StateVm>>($"{BaseUrl}/{ApiUrl}/state/by-countryId/{countryId}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownByCountryIdAsync(int countryId)
        {
            var vms = await GetAllByCountryIdAsync(countryId).ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }

        public async Task<ResponseMessageDto<StateVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<StateVm>($"{BaseUrl}/{ApiUrl}/state/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateStateVm>> GetForEditAsync(int id)
        {
            var vm = await GetAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateStateVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<StateVm>> PostAsync(CreateStateVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateStateDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<StateVm>($"{BaseUrl}/{ApiUrl}/state", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateStateVm model, int stateId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateStateDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/state/{stateId}", modelDto, true).ConfigureAwait(false);
        }
    }
}
