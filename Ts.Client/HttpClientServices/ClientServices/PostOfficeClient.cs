using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.PostOfficeVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.PostOfficeDataTableDtos;
using Ts.Dto.PostOfficeDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class PostOfficeClient : IPostOfficeClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public PostOfficeClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int id)
        {
            return await httpClientService.DeleteAsync<bool>($"{BaseUrl}/{ApiUrl}/postoffice/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<PostOfficeVm>>> GetAllAsync(PostOfficeDataTableRequestVm model)
        {
            var postModel = mapper.Map<PostOfficeDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<PostOfficeOrderDto>
                    {
                        OrderBy = new()
                    };

                    if (order.Column == 0)
                        sort.OrderBy.Name = true;
                    else if (order.Column == 5)
                        sort.OrderBy.CreatedOn = true;
                    else if (order.Column == 6)
                        sort.OrderBy.UpdatedOn = true;

                    if (order.Dir == "asc")
                        sort.IsAsc = true;
                    postModel.OrderList.Add(sort);
                }
            }
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<PostOfficeVm>>($"{BaseUrl}/{ApiUrl}/postoffice/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<IEnumerable<PostOfficeVm>>> GetAllByDistrictIdAsync(int districtId)
        {
            return await httpClientService.GetAsync<IEnumerable<PostOfficeVm>>($"{BaseUrl}/{ApiUrl}/postoffice/by-districtId/{districtId}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownByDistrictIdAsync(int districtId)
        {
            var vms = await GetAllByDistrictIdAsync(districtId).ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }

        public async Task<ResponseMessageDto<PostOfficeVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<PostOfficeVm>($"{BaseUrl}/{ApiUrl}/postoffice/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdatePostOfficeVm>> GetForEditAsync(int id)
        {
            var vm = await httpClientService.GetAsync<PostOfficeVm>($"{BaseUrl}/{ApiUrl}/postoffice/{id}/for-edit", true).ConfigureAwait(false);

            var updateVm = mapper.Map<ResponseMessageDto<UpdatePostOfficeVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<PostOfficeVm>> PostAsync(CreatePostOfficeVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreatePostOfficeDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<PostOfficeVm>($"{BaseUrl}/{ApiUrl}/postoffice", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdatePostOfficeVm model, int postOfficeId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdatePostOfficeDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/postoffice/{postOfficeId}", modelDto, true).ConfigureAwait(false);
        }
    }
}
