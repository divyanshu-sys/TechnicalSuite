using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.ClientUserDataTableDtos;
using Ts.ShopIn.Domain.DataTableModels;
using Ts.ShopIn.Domain.DataTableModels.BlogDataTables;
using Ts.ShopIn.Domain.DataTableModels.ClientUserDataTables;
using Ts.ShopIn.Domain.DataTableModels.OrderDataTables;
using Ts.ShopIn.Domain.DataTableModels.ProductDataTables;
using Ts.ShopIn.Domain.DataTableModels.ProductDetailDataTables;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.BlogImageDtos;
using Ts.ShopIn.Dto.BlogViewDtos;
using Ts.ShopIn.Dto.CartDtos;
using Ts.ShopIn.Dto.ClientUserDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.DataTableDtos.ProductDataTableDtos;
using Ts.ShopIn.Dto.DataTableDtos.ProductDetailDataTableDtos;
using Ts.ShopIn.Dto.OrderDetailDtos;
using Ts.ShopIn.Dto.OrderDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Dto.ProductDetailDtos;
using Ts.ShopIn.Dto.ProductDetailImageDtos;
using Ts.ShopIn.Dto.ProductDetailViewDtos;
using Ts.ShopIn.Dto.ProductDtos;
using Ts.ShopIn.Dto.ProductImageDtos;
using Ts.ShopIn.Dto.ProductVariantDtos;
namespace Ts.ShopIn.Service.AutoMapper
{
    public class AutoMapperProfileApi : Profile
    {
        public AutoMapperProfileApi(IConfiguration config)
        {
            #region DataTable
            CreateMap(typeof(SortOrderDto<>), typeof(SortOrder<>));
            CreateMap<SearchDto, Search>();

            CreateMap<BlogOrderDto, BlogOrder>();
            CreateMap<BlogDataTableRequestDto, BlogDataTableRequest>();
            CreateMap<BlogDataTableForViewRequestDto, BlogDataTableForViewRequest>();

            CreateMap<ClientUserOrderDto, ClientUserOrder>();
            CreateMap<ClientUserDataTableRequestDto, ClientUserDataTableRequest>();

            CreateMap<ProductOrderDto, ProductOrder>();
            CreateMap<ProductDataTableRequestDto, ProductDataTableRequest>();
            CreateMap<ProductDataTableForViewRequestDto, ProductDataTableForViewRequest>();

            CreateMap<ProductDetailOrderDto, ProductDetailOrder>();
            CreateMap<ProductDetailDataTableRequestDto, ProductDetailDataTableRequest>();
            CreateMap<ProductDetailDataTableForViewRequestDto, ProductDetailDataTableForViewRequest>();

            CreateMap<OrderDataTableForViewRequestDto, OrderDataTableForViewRequest>();
            #endregion

            #region Blog
            CreateMap<Blog, BlogDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:BlogImageUrl")}/{src.MainImage}"));
            CreateMap<Blog, GetBlogDataTableDto>();
            CreateMap<CreateBlogDto, Blog>();
            CreateMap<UpdateBlogDto, Blog>();
            CreateMap<UpdateBlogDescriptionDto, Blog>();
            CreateMap<UpdateBlogWorkerDto, Blog>();
            CreateMap<PublishBlogDto, Blog>();
            CreateMap<UpdateBlogMainImageDto, Blog>();
            CreateMap<Blog, GetUpdateBlogDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:BlogImageUrl")}/{src.MainImage}"));
            #endregion

            #region BlogView
            CreateMap<BlogView, BlogViewDto>();
            #endregion

            #region BlogImage
            CreateMap<BlogImage, BlogImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiShopIn:BlogImageUrl")));
            #endregion

            #region Product
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductImageUrl")}/{src.MainImage}"));
            CreateMap<Product, GetProductDataTableDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<UpdateProductWorkerDto, Product>();
            CreateMap<PublishProductDto, Product>();
            CreateMap<UpdateProductMainImageDto, Product>();
            CreateMap<Product, GetUpdateProductDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductImageUrl")}/{src.MainImage}"));
            #endregion

            #region ProductImage
            CreateMap<ProductImage, ProductImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiShopIn:ProductImageUrl")));
            #endregion

            #region ProductDetail
            CreateMap<ProductDetail, ProductDetailDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl")}/{src.MainImage}"));
            CreateMap<ProductDetail, GetProductDetailDataTableDto>();
            CreateMap<CreateProductDetailDto, ProductDetail>();
            CreateMap<UpdateProductDetailDto, ProductDetail>();
            CreateMap<UpdateProductDetailDescriptionDto, ProductDetail>();
            CreateMap<UpdateProductDetailWorkerDto, ProductDetail>();
            CreateMap<PublishProductDetailDto, ProductDetail>();
            CreateMap<UpdateProductDetailMainImageDto, ProductDetail>();
            CreateMap<ProductDetail, GetUpdateProductDetailDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl")}/{src.MainImage}"));
            CreateMap<ProductDetail, GetForViewProductDetailDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl")}/{src.MainImage}"));
            #endregion

            #region ProductDetailView
            CreateMap<ProductDetailView, ProductDetailViewDto>();
            #endregion

            #region ProductVariant
            CreateMap<ProductVariant, ProductVariantDto>();
            #endregion

            #region ProductDetailImage
            CreateMap<ProductDetailImage, ProductDetailImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl")));
            #endregion

            #region ProductDetailImage
            CreateMap<ProductDetailDocument, ProductDetailDocumentDto>();
            #endregion

            #region ClientUser
            CreateMap<RegisterClientUserDto, ClientUser>();
            CreateMap<ClientUser, ClientUserDto>();
            CreateMap<UpdateClientUserDto, ClientUser>();
            #endregion

            #region Cart
            CreateMap<CartDto, Cart>().ReverseMap();
            CreateMap<CreateCartDto, Cart>();
            #endregion

            #region Order
            CreateMap<Order, GetOrderForViewDto>();
            #endregion

            #region OrderDetail
            CreateMap<OrderDetail, GetOrderDetailForViewDto>();
            #endregion
        }
    }
}
