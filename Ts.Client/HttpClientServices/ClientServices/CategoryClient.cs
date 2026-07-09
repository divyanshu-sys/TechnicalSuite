using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.ViewModels.CategoryVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.CategoryDtos;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.CategoryDataTableDtos;
namespace Ts.Client.HttpClientServices.ClientServices
{
    public class CategoryClient : ICategoryClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpClientService httpClientService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string BaseUrl;

        public CategoryClient(IHttpClientService httpClientService, IConfiguration configuration,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientService = httpClientService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            BaseUrl = configuration["ApiBaseUri"];
        }

        public async Task<ResponseMessageDto<DataTableResponseVm<CategoryVm>>> GetAllAsync(CategoryDataTableRequestVm model)
        {
            var postModel = mapper.Map<CategoryDataTableRequestDto>(model);

            if (model.Order != null && model.Order.Any())
            {
                foreach (var order in model.Order)
                {
                    var sort = new SortOrderDto<CategoryOrderDto>
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
            var responseData = await httpClientService.PostAsync<DataTableResponseVm<CategoryVm>>($"{BaseUrl}/{ApiUrl}/category/datatable", postModel, true).ConfigureAwait(false);
            if (responseData.Data != null)
                responseData.Data.Draw = model.Draw;
            return responseData;
        }

        public async Task<ResponseMessageDto<IEnumerable<CategoryVm>>> GetAllAsync()
        {
            return await httpClientService.GetAsync<IEnumerable<CategoryVm>>($"{BaseUrl}/{ApiUrl}/category", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>> GetAllForDropDownAsync()
        {
            var vms = await GetAllAsync().ConfigureAwait(false);
            var dropDown = mapper.Map<ResponseMessageDto<IEnumerable<KeyValuePair<int, string>>>>(vms);
            return dropDown;
        }

        public async Task<ResponseMessageDto<CategoryVm>> GetAsync(int id)
        {
            return await httpClientService.GetAsync<CategoryVm>($"{BaseUrl}/{ApiUrl}/category/{id}", true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<UpdateCategoryVm>> GetForEditAsync(int id)
        {
            var vm = await GetAsync(id).ConfigureAwait(false);
            var updateVm = mapper.Map<ResponseMessageDto<UpdateCategoryVm>>(vm);
            return updateVm;
        }

        public async Task<ResponseMessageDto<CategoryVm>> PostAsync(CreateCategoryVm model)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<CreateCategoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PostAsync<CategoryVm>($"{BaseUrl}/{ApiUrl}/category", modelDto, true).ConfigureAwait(false);
        }

        public async Task<ResponseMessageDto<bool>> PutAsync(UpdateCategoryVm model, int categoryId)
        {
            var ipAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            var modelDto = mapper.Map<UpdateCategoryDto>(model);
            modelDto.IpAddress = ipAddress;
            return await httpClientService.PutAsync<bool>($"{BaseUrl}/{ApiUrl}/category/{categoryId}", modelDto, true).ConfigureAwait(false);
        }
    }
}
