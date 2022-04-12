using Microsoft.Extensions.DependencyInjection;

namespace FeatureFlagHelper;

public class ServiceRegistry
{
    public static IServiceCollection RegisterServices(ServiceCollection serviceCollection, Settings settings)
    {
        var a = serviceCollection.AddLogging()
            .AddSingleton(settings)
            .AddTransient<IJsonFileReader, JsonFileReader>()
            .AddTransient<IJsonFileWriter, JsonFileWriter>()
            .AddTransient<IFeatureFlagUpdater, FeatureFlagUpdater>();
        return a;
    }
}
