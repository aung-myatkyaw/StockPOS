using AutoMapper;

namespace AutoMapperConfig
{
    public static class AutoMapperConfig
    {
        public static void AddAutomapperConfiguration(this IServiceCollection services)
        {
            var automapperConfig = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new MappingProfile());
            });

            var autoMapper = automapperConfig.CreateMapper();

            services.AddSingleton(autoMapper);
        }
    }
}
