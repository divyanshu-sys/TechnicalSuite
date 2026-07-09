using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.HttpClientServices.ClientServices;
namespace Ts.PublicComApp.Configurations
{
    public static class ServiceContainer
    {
        public static void RegisterHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            services.AddScoped<IContactClient, ContactClient>();
            services.AddScoped<IPostForViewClient, PostForViewClient>();
            services.AddScoped<IStoryForViewClient, StoryForViewClient>();
        }
    }
}
