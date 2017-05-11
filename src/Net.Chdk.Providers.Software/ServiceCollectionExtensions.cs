using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSoftwareHashProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISoftwareHashProvider, SoftwareHashProvider>();
        }
    }
}
