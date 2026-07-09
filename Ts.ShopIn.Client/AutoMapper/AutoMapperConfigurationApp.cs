using AutoMapper;
namespace Ts.ShopIn.Client.AutoMapper
{
    public static class AutoMapperConfigurationApp
    {
        public static MapperConfiguration RegisterAutoMapperProfilesApp()
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileApp());
            });
        }
    }
}
