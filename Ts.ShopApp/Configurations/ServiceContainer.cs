using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
using Ts.ShopIn.Client.HttpClientServices.ClientServices;
namespace Ts.ShopApp.Configurations
{
    public static class ServiceContainer
    {
        public static void RegisterHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            services.AddScoped<IContactClient, ContactClient>();
            services.AddScoped<IBlogForViewClient, BlogForViewClient>();
            services.AddScoped<IClientUserClient, ClientUserClient>();
            services.AddScoped<IHomeClient, HomeClient>();
            services.AddScoped<IProductDetailForViewClient, ProductDetailForViewClient>();
            services.AddScoped<IProductForViewClient, ProductForViewClient>();
            services.AddScoped<ICartClient, CartClient>();
            services.AddScoped<IOrderForViewClient, OrderForViewClient>();
        }
    }
}
