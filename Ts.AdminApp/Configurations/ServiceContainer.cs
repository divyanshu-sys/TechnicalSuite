using Ts.Client.HttpClientServices.ClientInterfaces;
using Ts.Client.HttpClientServices.ClientServices;
using DotComClientInterfaces = Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using DotComClientServices = Ts.DotCom.Client.HttpClientServices.ClientServices;
using DotInClientInterfaces = Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using DotInClientServices = Ts.DotIn.Client.HttpClientServices.ClientServices;
using ShopInClientInterfaces = Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using ShopInClientServices = Ts.ShopIn.Client.HttpClientServices.ClientServices;
namespace Ts.AdminApp.Configurations
{
    public static class ServiceContainer
    {
        public static void RegisterHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDropDownClient, DropDownClient>();
            services.AddScoped<ICountryClient, CountryClient>();
            services.AddScoped<IApplicationUserClient, ApplicationUserClient>();
            services.AddScoped<IStateClient, StateClient>();
            services.AddScoped<IDistrictClient, DistrictClient>();
            services.AddScoped<IPostOfficeClient, PostOfficeClient>();
            services.AddScoped<ICategoryClient, CategoryClient>();
            services.AddScoped<ISubCategoryClient, SubCategoryClient>();
            services.AddScoped<IShopCategoryClient, ShopCategoryClient>();
            services.AddScoped<DotInClientInterfaces.IPostClient, DotInClientServices.PostClient>();
            services.AddScoped<DotComClientInterfaces.IPostClient, DotComClientServices.PostClient>();
            services.AddScoped<DotInClientInterfaces.IStoryClient, DotInClientServices.StoryClient>();
            services.AddScoped<DotComClientInterfaces.IStoryClient, DotComClientServices.StoryClient>();
            services.AddScoped<ShopInClientInterfaces.IBlogClient, ShopInClientServices.BlogClient>();
            services.AddScoped<ShopInClientInterfaces.IProductClient, ShopInClientServices.ProductClient>();
            services.AddScoped<ShopInClientInterfaces.IProductDetailClient, ShopInClientServices.ProductDetailClient>();
            services.AddScoped<ShopInClientInterfaces.IOrderClient, ShopInClientServices.OrderClient>();
        }
    }
}
