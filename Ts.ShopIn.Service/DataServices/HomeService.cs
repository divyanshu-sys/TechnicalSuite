using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Dto.HomeDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.ShopIn.Service.DataServices
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ISubCategoryService subCategoryService;
        private readonly IConfiguration config;
        private readonly IShopCategoryService shopCategoryService;
        private readonly IDropDownService dropDownService;

        public HomeService(IUnitOfWork unitOfWork, IMapper mapper, ISubCategoryService subCategoryService,
            IConfiguration config, IShopCategoryService shopCategoryService,
            IDropDownService dropDownService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.subCategoryService = subCategoryService;
            this.config = config;
            this.shopCategoryService = shopCategoryService;
            this.dropDownService = dropDownService;
        }

        public async Task<DisplayHomeItemsDto> DisplayItemsAsync()
        {
            var result = new DisplayHomeItemsDto
            {
                BlogForListView = await CommonBlogService.GetForListViewAsync(unitOfWork, mapper, subCategoryService, config, new() { Start = 0, Length = 6 }).ConfigureAwait(false),
                ProductForListView = await CommonProductService.GetForListViewAsync(unitOfWork, mapper, shopCategoryService, config, new() { Start = 0, Length = 6 }, dropDownService).ConfigureAwait(false),
                ProductDetailForListView = await CommonProductDetailService.GetForListViewAsync(unitOfWork, mapper, shopCategoryService, config, new() { Start = 0, Length = 6 }, dropDownService).ConfigureAwait(false),
            };
            return result;
        }
    }
}
