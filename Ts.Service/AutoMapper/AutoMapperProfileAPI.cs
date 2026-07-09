using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Domain.DataTableModels;
using Ts.Domain.DataTableModels.ApplicationUserDataTables;
using Ts.Domain.DataTableModels.CategoryDataTables;
using Ts.Domain.DataTableModels.CountryDataTables;
using Ts.Domain.DataTableModels.DistrictDataTables;
using Ts.Domain.DataTableModels.PostOfficeDataTables;
using Ts.Domain.DataTableModels.ShopCategoryDataTables;
using Ts.Domain.DataTableModels.StateDataTables;
using Ts.Domain.DataTableModels.SubCategoryDataTables;
using Ts.Domain.Models;
using Ts.Dto.AddressDtos;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.CategoryDtos;
using Ts.Dto.CountryDtos;
using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.ApplicationUserDataTableDtos;
using Ts.Dto.DataTableDtos.CategoryDataTableDtos;
using Ts.Dto.DataTableDtos.CountryDataTableDtos;
using Ts.Dto.DataTableDtos.DistrictDataTableDtos;
using Ts.Dto.DataTableDtos.PostOfficeDataTableDtos;
using Ts.Dto.DataTableDtos.ShopCategoryDataTableDtos;
using Ts.Dto.DataTableDtos.StateDataTableDtos;
using Ts.Dto.DataTableDtos.SubCategoryDataTableDtos;
using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.DistrictDtos;
using Ts.Dto.ExchangePolicyDtos;
using Ts.Dto.OrderStatusDtos;
using Ts.Dto.PaymentGatewayTypeDtos;
using Ts.Dto.PaymentModeDtos;
using Ts.Dto.PaymentStatusDtos;
using Ts.Dto.PostOfficeDtos;
using Ts.Dto.RefreshTokenDtos;
using Ts.Dto.ReturnPolicyDtos;
using Ts.Dto.ShopCategoryDtos;
using Ts.Dto.StateDtos;
using Ts.Dto.SubCategoryDtos;
namespace Ts.Service.AutoMapper
{
    public class AutoMapperProfileApi : Profile
    {
        public AutoMapperProfileApi(IConfiguration config)
        {
            CreateMap<DateTimeOffset, DateTime>().ConvertUsing(src => src.UtcDateTime);

            #region Country
            CreateMap<Country, CountryDto>();
            CreateMap<CreateCountryDto, Country>();
            CreateMap<UpdateCountryDto, Country>();
            #endregion

            #region State
            CreateMap<State, StateDto>();
            CreateMap<State, GetStateDataTableDto>();
            CreateMap<CreateStateDto, State>();
            CreateMap<UpdateStateDto, State>();
            CreateMap<State, GetProfileStateDto>();
            #endregion

            #region District
            CreateMap<District, DistrictDto>();
            CreateMap<District, GetDistrictDataTableDto>();
            CreateMap<District, GetUpdateDistrictDto>();
            CreateMap<CreateDistrictDto, District>();
            CreateMap<UpdateDistrictDto, District>();
            CreateMap<District, GetProfileDistrictDto>();
            #endregion

            #region PostOffice
            CreateMap<PostOffice, PostOfficeDto>();
            CreateMap<PostOffice, GetPostOfficeDataTableDto>();
            CreateMap<PostOffice, GetUpdatePostOfficeDto>();
            CreateMap<CreatePostOfficeDto, PostOffice>();
            CreateMap<UpdatePostOfficeDto, PostOffice>();
            CreateMap<PostOffice, GetProfilePostOfficeDto>();
            #endregion

            #region RefreshToken
            CreateMap<RefreshToken, RefreshTokenDto>();
            #endregion

            #region ApplicationUser
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<RegisterApplicationUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetUpdateApplicationUserDto>();
            CreateMap<UpdateApplicationUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetProfileDto>();
            CreateMap<ApplicationUser, GetApplicationUserPrivilegeDto>();
            #endregion

            #region Address
            CreateMap<Address, AddressDto>();
            CreateMap<CreateAddressDto, Address>();
            CreateMap<Address, GetUpdateAddressDto>();
            CreateMap<UpdateAddressDto, Address>();
            CreateMap<Address, GetProfileAddressDto>();
            #endregion

            #region DataTable
            CreateMap(typeof(SortOrderDto<>), typeof(SortOrder<>));
            CreateMap<SearchDto, Search>();

            CreateMap<CountryOrderDto, CountryOrder>();
            CreateMap<CountryDataTableRequestDto, CountryDataTableRequest>();

            CreateMap<StateOrderDto, StateOrder>();
            CreateMap<StateDataTableRequestDto, StateDataTableRequest>();

            CreateMap<DistrictOrderDto, DistrictOrder>();
            CreateMap<DistrictDataTableRequestDto, DistrictDataTableRequest>();

            CreateMap<PostOfficeOrderDto, PostOfficeOrder>();
            CreateMap<PostOfficeDataTableRequestDto, PostOfficeDataTableRequest>();

            CreateMap<CategoryOrderDto, CategoryOrder>();
            CreateMap<CategoryDataTableRequestDto, CategoryDataTableRequest>();

            CreateMap<SubCategoryOrderDto, SubCategoryOrder>();
            CreateMap<SubCategoryDataTableRequestDto, SubCategoryDataTableRequest>();

            CreateMap<ShopCategoryOrderDto, ShopCategoryOrder>();
            CreateMap<ShopCategoryDataTableRequestDto, ShopCategoryDataTableRequest>();

            CreateMap<ApplicationUserOrderDto, ApplicationUserOrder>();
            CreateMap<ApplicationUserDataTableRequestDto, ApplicationUserDataTableRequest>();
            #endregion

            #region Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            #endregion

            #region SubCategory
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<CreateSubCategoryDto, SubCategory>();
            CreateMap<UpdateSubCategoryDto, SubCategory>();
            #endregion

            #region ShopCategory
            CreateMap<ShopCategory, ShopCategoryDto>();
            CreateMap<CreateShopCategoryDto, ShopCategory>();
            CreateMap<UpdateShopCategoryDto, ShopCategory>();
            #endregion

            #region ExchangePolicy
            CreateMap<ExchangePolicy, ExchangePolicyDto>();
            #endregion

            #region DeliveryPolicy
            CreateMap<DeliveryPolicy, DeliveryPolicyDto>();
            #endregion

            #region ReturnPolicy
            CreateMap<ReturnPolicy, ReturnPolicyDto>();
            #endregion

            #region PaymentMode
            CreateMap<PaymentMode, PaymentModeDto>();
            #endregion

            #region OrderStatus
            CreateMap<OrderStatus, OrderStatusDto>();
            #endregion

            #region PaymentGatewayType
            CreateMap<PaymentGatewayType, PaymentGatewayTypeDto>();
            #endregion

            #region PaymentStatus
            CreateMap<PaymentStatus, PaymentStatusDto>();
            #endregion

            #region CurrencyType
            CreateMap<CurrencyType, CurrencyTypeDto>();
            #endregion
        }
    }
}
