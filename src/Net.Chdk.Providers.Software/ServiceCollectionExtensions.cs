using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSourceProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISourceProvider, SourceProvider>();
        }

        public static IServiceCollection AddSoftwareHashProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISoftwareHashProvider, SoftwareHashProvider>();
        }

        public static IServiceCollection AddCameraProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraProvider, CameraProvider>();
        }

        public static IServiceCollection AddEncodingProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEncodingProvider, EncodingProvider>();
        }

        public static IServiceCollection AddModulesProviderResolver(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IModulesProviderResolver, ModulesProviderResolver>();
        }
    }
}
