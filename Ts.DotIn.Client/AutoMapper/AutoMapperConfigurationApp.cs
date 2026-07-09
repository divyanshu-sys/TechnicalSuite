using AutoMapper;
namespace Ts.DotIn.Client.AutoMapper
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
