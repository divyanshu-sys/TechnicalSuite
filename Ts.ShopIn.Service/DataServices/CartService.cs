using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Application.HelperExtensions;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.CartDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.ShopIn.Service.DataServices
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDropDownService dropDownService;
        private readonly IShopCategoryService shopCategoryService;
        private readonly IConfiguration configuration;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper, IDropDownService dropDownService,
            IShopCategoryService shopCategoryService,
            IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.dropDownService = dropDownService;
            this.shopCategoryService = shopCategoryService;
            this.configuration = configuration;
        }

        public async Task<ResponseMessageDto<CartDto>> CreateAsync(CreateCartDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<CartDto>();

            modelDto.HtmlEncodeObject();
            var cart = await unitOfWork.CartRepo.GetAsync(modelDto.ProductDetailId, userId).ConfigureAwait(false);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            if (cart != null)
            {
                var deliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == cart.ProductDetail.ShopCategoryId);
                var isDownloadable = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies).Contains(deliveryPolicy.Id);
                if ((!cart.ProductDetail.IsAvailable || !cart.ProductDetail.IsPublished) && cart.ProductDetail.Stock == 0 ||
                    !isDownloadable)
                {
                    responseResult.ErrorMessage.Add("This item is no longer available.");
                    return responseResult;
                }
                if (!isDownloadable)
                {
                    mapper.Map(modelDto, cart);
                    cart.UpdatedOn = DateTime.UtcNow;
                }
                else
                    responseResult.ErrorMessage.Add("This item is already added to your cart.");
                return responseResult;
            }
            else
            {
                cart = mapper.Map<Cart>(modelDto);
                cart.UserId = userId;
                cart.CreatedOn = DateTime.UtcNow;
                cart.IsActive = true;

                var productDetail = await unitOfWork.ProductDetailRepo.GetAsync(cart.ProductDetailId).ConfigureAwait(false);
                if (productDetail == null)
                {
                    responseResult.ErrorMessage.Add("Item not found.");
                    return responseResult;
                }

                var deliveryPolicy = deliveryPolicies.SingleOrDefault(x => x.Id == productDetail.DeliveryPolicyId);
                var isDownloadable = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies).Contains(deliveryPolicy.Id);

                if ((!productDetail.IsAvailable || !productDetail.IsPublished) && productDetail.Stock == 0 ||
                    !isDownloadable)
                {
                    responseResult.ErrorMessage.Add("This item is no longer available.");
                    return responseResult;
                }

                if (isDownloadable)
                {
                    cart.ItemCount = 1;
                }
            }

            unitOfWork.CartRepo.Create(cart);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            responseResult.Data = mapper.Map<CartDto>(cart);
            return responseResult;
        }

        public async Task<GetCartForListViewDto> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = await CartCommonService.GetCartByUserIdAsync(unitOfWork, shopCategoryService, configuration, userId).ConfigureAwait(false);

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> DeleteAsync(int productDetailId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var entity = await unitOfWork.CartRepo.GetAsync(productDetailId, userId).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("Cart item you want to remove was not found.");
                return responseResult;
            }

            unitOfWork.CartRepo.Delete(entity);
            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            if (rowsChanged > 0)
                responseResult.Data = true;

            return responseResult;
        }
    }
}
