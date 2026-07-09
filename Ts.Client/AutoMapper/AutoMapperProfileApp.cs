using AutoMapper;
using Ts.Client.ViewModels.AddressVms;
using Ts.Client.ViewModels.ApplicationUserVms;
using Ts.Client.ViewModels.CategoryVms;
using Ts.Client.ViewModels.CountryVms;
using Ts.Client.ViewModels.DataTableVms;
using Ts.Client.ViewModels.DistrictVms;
using Ts.Client.ViewModels.PostOfficeVms;
using Ts.Client.ViewModels.ShopCategoryVms;
using Ts.Client.ViewModels.StateVms;
using Ts.Client.ViewModels.SubCategoryVms;
using Ts.Dto;
using Ts.Dto.AddressDtos;
using Ts.Dto.ApplicationUserDtos;
using Ts.Dto.CategoryDtos;
using Ts.Dto.CountryDtos;
using Ts.Dto.DataTableDtos;
using Ts.Dto.DataTableDtos.ApplicationUserDataTableDtos;
using Ts.Dto.DataTableDtos.CategoryDataTableDtos;
using Ts.Dto.DataTableDtos.ClientUserDataTableDtos;
using Ts.Dto.DataTableDtos.CountryDataTableDtos;
using Ts.Dto.DataTableDtos.DistrictDataTableDtos;
using Ts.Dto.DataTableDtos.PostOfficeDataTableDtos;
using Ts.Dto.DataTableDtos.ShopCategoryDataTableDtos;
using Ts.Dto.DataTableDtos.StateDataTableDtos;
using Ts.Dto.DataTableDtos.SubCategoryDataTableDtos;
using Ts.Dto.DistrictDtos;
using Ts.Dto.PostOfficeDtos;
using Ts.Dto.ShopCategoryDtos;
using Ts.Dto.StateDtos;
using Ts.Dto.SubCategoryDtos;
namespace Ts.Client.AutoMapper
{
    public class AutoMapperProfileApp : Profile
    {
        public AutoMapperProfileApp()
        {
            #region DataTable
            CreateMap(typeof(SearchVm), typeof(SearchDto));

            CreateMap<ApplicationUserDataTableRequestVm, ApplicationUserDataTableRequestDto>();
            CreateMap<ClientUserDataTableRequestVm, ClientUserDataTableRequestDto>();
            CreateMap<CategoryDataTableRequestVm, CategoryDataTableRequestDto>();
            CreateMap<CountryDataTableRequestVm, CountryDataTableRequestDto>();
            CreateMap<DistrictDataTableRequestVm, DistrictDataTableRequestDto>();
            CreateMap<PostOfficeDataTableRequestVm, PostOfficeDataTableRequestDto>();
            CreateMap<StateDataTableRequestVm, StateDataTableRequestDto>();
            CreateMap<SubCategoryDataTableRequestVm, SubCategoryDataTableRequestDto>();
            CreateMap<ShopCategoryDataTableRequestVm, ShopCategoryDataTableRequestDto>();
            #endregion

            #region Application User
            CreateMap<LoginVm, LoginDto>();
            CreateMap<ChangePasswordVm, ChangePasswordDto>();
            CreateMap<ResetPasswordVm, ResetPasswordDto>();
            CreateMap<RegisterApplicationUserVm, RegisterApplicationUserDto>();
            CreateMap<ApplicationUserVm, UpdateApplicationUserVm>();
            CreateMap<UpdateApplicationUserVm, UpdateApplicationUserDto>();
            CreateMap<UpdatePrivilegeVm, UpdateApplicationUserPrivilegeDto>()
            .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.UserRoles.Where(r => r.IsSelected).Select(r => r.Name)))
            .ForMember(dest => dest.ClaimTypes, opt => opt.MapFrom(src => src.UserClaims.SelectMany(c => c.Policies.SelectMany(p => p.Claims.Where(c => c.IsSelected).Select(cl => cl.ClaimType)))));
            CreateMap<ApplicationUserVm, KeyValuePair<string, string>>()
                .ConvertUsing(vm => new KeyValuePair<string, string>(vm.Id, $"{vm.FirstName} {vm.LastName}".Trim()));
            #endregion

            CreateMap(typeof(ResponseMessageDto<>), typeof(ResponseMessageDto<>));

            #region Country
            CreateMap<CreateCountryVm, CreateCountryDto>();
            CreateMap<UpdateCountryVm, UpdateCountryDto>();
            CreateMap<CountryVm, UpdateCountryVm>();
            CreateMap<CountryVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, vm.Name));
            #endregion

            #region State
            CreateMap<CreateStateVm, CreateStateDto>();
            CreateMap<UpdateStateVm, UpdateStateDto>();
            CreateMap<StateVm, UpdateStateVm>();
            CreateMap<StateVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, vm.Name));
            #endregion

            #region District
            CreateMap<CreateDistrictVm, CreateDistrictDto>();
            CreateMap<UpdateDistrictVm, UpdateDistrictDto>();
            CreateMap<DistrictVm, UpdateDistrictVm>()
                .ForMember(dest => dest.CountryId, src => src.MapFrom(x => x.State.CountryId));
            CreateMap<DistrictVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, vm.Name));
            #endregion

            #region PostOffice
            CreateMap<CreatePostOfficeVm, CreatePostOfficeDto>();
            CreateMap<UpdatePostOfficeVm, UpdatePostOfficeDto>();
            CreateMap<PostOfficeVm, UpdatePostOfficeVm>()
                .ForMember(dest => dest.StateId, src => src.MapFrom(x => x.District.StateId))
                .ForMember(dest => dest.CountryId, src => src.MapFrom(x => x.District.State.CountryId));
            CreateMap<PostOfficeVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, $"{vm.Name} ({vm.Pincode})"));
            #endregion

            #region Address
            CreateMap<CreateAddressVm, CreateAddressDto>();
            CreateMap<AddressVm, UpdateAddressVm>()
                .ForMember(dest => dest.DistrictId, src => src.MapFrom(x => x.PostOffice.DistrictId))
                .ForMember(dest => dest.StateId, src => src.MapFrom(x => x.PostOffice.District.StateId))
                .ForMember(dest => dest.CountryId, src => src.MapFrom(x => x.PostOffice.District.State.CountryId));
            CreateMap<UpdateAddressVm, UpdateAddressDto>();
            #endregion

            #region Category
            CreateMap<CreateCategoryVm, CreateCategoryDto>();
            CreateMap<UpdateCategoryVm, UpdateCategoryDto>();
            CreateMap<CategoryVm, UpdateCategoryVm>();
            CreateMap<CategoryVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, vm.Name));
            #endregion

            #region SubCategory
            CreateMap<CreateSubCategoryVm, CreateSubCategoryDto>();
            CreateMap<UpdateSubCategoryVm, UpdateSubCategoryDto>();
            CreateMap<SubCategoryVm, UpdateSubCategoryVm>();
            CreateMap<SubCategoryVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, vm.Name));
            #endregion

            #region ShopCategory
            CreateMap<CreateShopCategoryVm, CreateShopCategoryDto>();
            CreateMap<UpdateShopCategoryVm, UpdateShopCategoryDto>();
            CreateMap<ShopCategoryVm, UpdateShopCategoryVm>();
            CreateMap<ShopCategoryVm, KeyValuePair<int, string>>()
                .ConvertUsing(vm => new KeyValuePair<int, string>(vm.Id, vm.Name));
            #endregion
        }
    }
}
