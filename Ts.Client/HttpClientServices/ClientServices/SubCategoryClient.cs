using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.SubCategoryVms;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.SubCategoryDataTableDtos;
using Ts.Dto.SubCategoryDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class SubCategoryClient : ISubCategoryClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public SubCategoryClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<SubCategoryVm>>> GetAllAsync(SubCategoryDataTableRequestVm model)
        {
            var postModel = mapper.Map<SubCategoryDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<SubCategoryOrderDto>
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
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<SubCategoryVm>>($"{BaseUrl}/{ApiUrl}/subcategory/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<IEnumerable<SubCategoryVm>>> GetAllAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<SubCategoryVm>>($"{BaseUrl}/{ApiUrl}/subcategory", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync()
        {
            var vms = await GetAllAsync().ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }

        public async Task<ResponseMessageDto<SubCategoryVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<SubCategoryVm>($"{BaseUrl}/{ApiUrl}/subcategory/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateSubCategoryVm>> GetForEditAsync(int id)
        {
            var vm = await GetAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateSubCategoryVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<SubCategoryVm>> PostAsync(CreateSubCategoryVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateSubCategoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<SubCategoryVm>($"{BaseUrl}/{ApiUrl}/subcategory", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateSubCategoryVm model, int subCategoryId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateSubCategoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/subcategory/{subCategoryId}", modelDto, true).ConfigureAwait(false);
        }
    }
}
