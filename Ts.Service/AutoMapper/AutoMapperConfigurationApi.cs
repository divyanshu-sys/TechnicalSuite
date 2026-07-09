using AutoMapper;
using Microsoft.Extensions.Configuration;
namespace Ts.Service.AutoMapper
{
    public static class AutoMapperConfigurationApi
    {
        public static MapperConfiguration RegisterAutoMapperProfilesApi(IConfiguration configuration)
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileApi(configuration));
            });
        }
    }
}
