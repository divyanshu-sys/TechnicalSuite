using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.CountryVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.CountryDtos;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.CountryDataTableDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class CountryClient : ICountryClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public CountryClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<CountryVm>>> GetAllAsync(CountryDataTableRequestVm model)
        {
            var postModel = mapper.Map<CountryDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<CountryOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 0)
                        sort.OrderBy.Name = true;
                    else if (order.Column == 1)
                        sort.OrderBy.Code2 = true;
                    else if (order.Column == 2)
                        sort.OrderBy.Code3 = true;
                    else if (order.Column == 3)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 4)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<CountryVm>>($"{BaseUrl}/{ApiUrl}/country/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<CountryVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<CountryVm>($"{BaseUrl}/{ApiUrl}/country/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateCountryVm>> GetForEditAsync(int id)
        {
            var vm = await GetAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateCountryVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<CountryVm>> PostAsync(CreateCountryVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateCountryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<CountryVm>($"{BaseUrl}/{ApiUrl}/country", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateCountryVm model, int countryId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateCountryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/country/{countryId}", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/country/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<CountryVm>>> GetAllAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<CountryVm>>($"{BaseUrl}/{ApiUrl}/country", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync()
        {
            var vms = await GetAllAsync().ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }
    }
}
