using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Common.Constant.SiteConstants;
using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.ShopCategoryDtos;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;

namespace Ts.ShopIn.Service.DataServices
{
    public static class CommonProductDetailService
    {
        public static async Task<List<GetProductDetailForListViewDto>> GetForListViewAsync(IUnitOfWork unitOfWork, IMapper mapper, IShopCategoryService shopCategoryService, IConfiguration config, ProductDetailDataTableForViewRequestDto modelDto, IDropDownService dropDownService)
        {
            var dtos = new List<GetProductDetailForListViewDto>();

            var model = mapper.Map<ProductDetailDataTableForViewRequest>(modelDto);

            var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(modelDto.ShopCategoryName))
            {
                model.ShopCategoryId = shopCategories.FirstOrDefault(x => x.Name.Equals(modelDto.ShopCategoryName, StringComparison.OrdinalIgnoreCase))?.Id;
                if (model.ShopCategoryId == null)
                    return dtos;
            }

            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);

            List<int> downloadableIds = GetDownloadableIdsForDeliveryPolicy(deliveryPolicies);

            var entities = await unitOfWork.ProductDetailRepo.GetForListViewAsync(model, downloadableIds).ConfigureAwait(false);

            foreach (var entity in entities)
            {
                GetProductDetailForListViewDto dto = SetProductDetailForListViewDto(config, shopCategories, entity);
                dtos.Add(dto);
            }

            return dtos;
        }

        public static GetProductDetailForListViewDto SetProductDetailForListViewDto(IConfiguration config, IEnumerable<ShopCategoryDto> shopCategories, ProductDetail entity)
        {
            var productDetail = new GetProductDetailForListViewDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ShopCategoryId = entity.ShopCategoryId,
                ShopCategoryName = shopCategories.FirstOrDefault(x => x.Id == entity.ShopCategoryId).Name,
                ProductDetailLink = entity.ProductDetailLink,
                MainImageUrl = entity.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl")}/{entity.MainImage}",
                PublishedOn = entity.PublishedOn,
                IsAvailable = entity.IsAvailable && entity.IsPublished,
                ExchangePolicyId = entity.ExchangePolicyId,
                DeliveryPolicyId = entity.DeliveryPolicyId,
                ReturnPolicyId = entity.ReturnPolicyId,
            };
            productDetail.Mrp = RoundingToDecimal(entity.Mrp, productDetail.CurrencyLetter);
            productDetail.Price = RoundingToDecimal(entity.Price, productDetail.CurrencyLetter);
            return productDetail;
        }

        public static List<int> GetDownloadableIdsForDeliveryPolicy(IEnumerable<DeliveryPolicyDto> deliveryPolicies)
        {
            return deliveryPolicies.Where(x => x.Name.Equals(DeliveryPolicyConstant.Downloadable15Days, StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).ToList();
        }

        public static decimal RoundingToDecimal(decimal totalAmount, string currencyLetter)
        {
            if (currencyLetter.Equals(CurrencyTypeConstant.INR, StringComparison.OrdinalIgnoreCase))
            {
                return Math.Round(totalAmount, 2);
            }
            return Math.Round(totalAmount);
        }

        public static long RoundingToInteger(decimal totalAmount, string currencyLetter)
        {
            if (currencyLetter.Equals(CurrencyTypeConstant.INR, StringComparison.OrdinalIgnoreCase))
            {
                return (long)(totalAmount * 100);
            }
            return (long)totalAmount;

        }

        public static decimal ConvertToDecimal(long totalAmount, string currencyLetter)
        {
            if (currencyLetter.Equals(CurrencyTypeConstant.INR, StringComparison.OrdinalIgnoreCase))
            {
                return totalAmount / 100m;
            }
            return totalAmount;
        }
    }
}
