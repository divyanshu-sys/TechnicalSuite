using Ts.DotIn.Client.HttpClientServices.ClientInterfaces;
using Ts.DotIn.Client.HttpClientServices.ClientServices;
namespace Ts.PublicApp.Configurations
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
