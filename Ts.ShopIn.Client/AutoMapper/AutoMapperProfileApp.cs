using AutoMapper;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.ClientUserVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Dto;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.DataTableDtos;
using Ts.ShopIn.Client.ViewModels;
using Ts.ShopIn.Client.ViewModels.BlogVms;
using Ts.ShopIn.Client.ViewModels.CartVms;
using Ts.ShopIn.Client.ViewModels.ClientUserVms;
using Ts.ShopIn.Client.ViewModels.DataTableVms;
using Ts.ShopIn.Client.ViewModels.ProductDetailVms;
using Ts.ShopIn.Client.ViewModels.ProductVms;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.CartDtos;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
using Ts.ShopIn.Dto.ProductDtos;
namespace Ts.ShopIn.Client.AutoMapper
{
    public class AutoMapperProfileApp : Profile
    {
        public AutoMapperProfileApp()
        {
            #region DataTable
            CreateMap(typeof(SearchVm), typeof(SearchDto));

            CreateMap<BlogDataTableRequestVm, BlogDataTableRequestDto>();
            CreateMap<ProductDataTableRequestVm, ProductDataTableRequestDto>();
            CreateMap<ProductDetailDataTableRequestVm, ProductDetailDataTableRequestDto>();
            #endregion

            CreateMap(typeof(ResponseMessageDto<>), typeof(ResponseMessageDto<>));

            #region Blog
            CreateMap<UpdateBlogVm, UpdateBlogDto>();
            CreateMap<CreateBlogVm, CreateBlogDto>();
            CreateMap<BlogVm, UpdateBlogVm>();
            CreateMap<UpdateBlogDescriptionVm, UpdateBlogDescriptionDto>();
            CreateMap<BlogVm, UpdateBlogDescriptionVm>();
            CreateMap<BlogVm, UpdateBlogMainImageVm>();
            CreateMap<BlogVm, UpdateBlogWorkerVm>();
            CreateMap<UpdateBlogWorkerVm, UpdateBlogWorkerDto>();
            CreateMap<UpdateBlogMainImageVm, UpdateBlogMainImageDto>();
            #endregion

            #region Product
            CreateMap<UpdateProductVm, UpdateProductDto>();
            CreateMap<CreateProductVm, CreateProductDto>();
            CreateMap<ProductVm, UpdateProductVm>();
            CreateMap<ProductVm, UpdateProductMainImageVm>();
            CreateMap<ProductVm, UpdateProductWorkerVm>();
            CreateMap<UpdateProductWorkerVm, UpdateProductWorkerDto>();
            CreateMap<UpdateProductMainImageVm, UpdateProductMainImageDto>();
            #endregion

            #region ProductDetail
            CreateMap<UpdateProductDetailVm, UpdateProductDetailDto>();
            CreateMap<CreateProductDetailVm, CreateProductDetailDto>();
            CreateMap<ProductDetailVm, UpdateProductDetailVm>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductVariant == null ? (int?)null : src.ProductVariant.ProductId));
            CreateMap<UpdateProductDetailDescriptionVm, UpdateProductDetailDescriptionDto>();
            CreateMap<ProductDetailVm, UpdateProductDetailDescriptionVm>();
            CreateMap<ProductDetailVm, UpdateProductDetailMainImageVm>();
            CreateMap<ProductDetailVm, UpdateProductDetailWorkerVm>();
            CreateMap<UpdateProductDetailWorkerVm, UpdateProductDetailWorkerDto>();
            CreateMap<UpdateProductDetailMainImageVm, UpdateProductDetailMainImageDto>();
            #endregion

            #region ClientUser
            CreateMap<LoginVm, LoginDto>();
            CreateMap<ChangePasswordVm, ChangePasswordDto>();
            CreateMap<ResetPasswordVm, ResetPasswordDto>();
            CreateMap<RegisterClientUserVm, RegisterClientUserDto>();
            CreateMap<ClientUserVm, UpdateClientUserVm>();
            CreateMap<UpdateClientUserVm, UpdateClientUserDto>();
            #endregion

            CreateMap<ContactFormVm, ContactFormDto>();

            #region Cart
            CreateMap<CreateCartVm, CreateCartDto>();
            #endregion
        }
    }
}
