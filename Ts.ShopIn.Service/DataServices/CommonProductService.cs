using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.DataTableModels.ProductDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.ProductDtos;

namespace Ts.ShopIn.Service.DataServices
{
    public static class CommonProductService
    {
        public static async Task<List<GetProductForListViewDto>> GetForListViewAsync(IUnitOfWork unitOfWork, IMapper mapper, IShopCategoryService shopCategoryService, IConfiguration config, ProductDataTableForViewRequestDto modelDto, IDropDownService dropDownService)
        {
            var dtos = new List<GetProductForListViewDto>();

            var model = mapper.Map<ProductDataTableForViewRequest>(modelDto);

            var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);

            var downloadableIds = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies);
            var entities = await unitOfWork.ProductRepo.GetForListViewAsync(model, downloadableIds).ConfigureAwait(false);

            foreach (var entity in entities)
            {
                var dto = new GetProductForListViewDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    ShopCategoryId = entity.ProductVariants.Single().ProductDetail.ShopCategoryId,
                    ShopCategoryName = shopCategories.FirstOrDefault(x => x.Id == entity.ProductVariants.Single().ProductDetail.ShopCategoryId).Name,
                    ProductDetailLink = entity.ProductVariants.Single().ProductDetail.ProductDetailLink,
                    MainImageUrl = entity.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductImageUrl")}/{entity.MainImage}",
                    PublishedOn = entity.PublishedOn
                };
                dto.Mrp = CommonProductDetailService.RoundingToDecimal(entity.ProductVariants.Single().ProductDetail.Mrp, dto.CurrencyLetter);
                dto.Price = CommonProductDetailService.RoundingToDecimal(entity.ProductVariants.Single().ProductDetail.Price, dto.CurrencyLetter);
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
