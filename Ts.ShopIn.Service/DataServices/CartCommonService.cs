using Microsoft.Extensions.Configuration;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Dto.CartDtos;

namespace Ts.ShopIn.Service.DataServices
{
    public static class CartCommonService
    {
        public static async Task<GetCartForListViewDto> GetCartByUserIdAsync(IUnitOfWork unitOfWork, IShopCategoryService shopCategoryService, IConfiguration configuration, string userId)
        {
            var responseResult = new GetCartForListViewDto();
            var entities = await unitOfWork.CartRepo.GetByUserIdAsync(userId).ConfigureAwait(false);

            if (entities.Count > 0)
            {
                var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);

                foreach (var item in entities)
                {
                    var detail = CommonProductDetailService.SetProductDetailForListViewDto(configuration, shopCategories, item.ProductDetail);

                    responseResult.TotalAvailableItemAmount += item.ItemCount * (detail.IsAvailable ? detail.Price : 0);
                    responseResult.TotalAvailableItemCount += detail.IsAvailable ? item.ItemCount : 0;

                    responseResult.ProductDetails.Add(new()
                    {
                        ProductDetailId = item.ProductDetailId,
                        ItemCount = item.ItemCount,
                        TotalPrice = item.ItemCount * (detail.IsAvailable ? detail.Price : 0),
                        UserId = item.UserId,
                        ProductDetail = detail
                    });
                }
            }

            return responseResult;
        }
    }
}
