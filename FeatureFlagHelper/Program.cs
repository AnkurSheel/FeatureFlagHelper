// See https://aka.ms/new-console-template for more information

using FeatureFlagHelper;
using Microsoft.Extensions.DependencyInjection;

var settings = new Settings(@"C:\SourceCode\CDD");

var serviceProvider = new ServiceCollection().AddLogging()
    .AddSingleton(settings)
    .AddTransient<IJsonFileReader, JsonFileReader>()
    .AddTransient<IJsonFileWriter, JsonFileWriter>()
    .BuildServiceProvider();
