// See https://aka.ms/new-console-template for more information

using FeatureFlagHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

var settings = new Settings(configuration["enumPath"], configuration["jsonFilesDirectory"]);

var serviceProvider = ServiceRegistry.RegisterServices(new ServiceCollection(), settings).BuildServiceProvider();

var action = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What do you want to do?").AddChoices(Actions.AddFlag, Actions.RemoveFlag));

var featureFlagName = AnsiConsole.Ask<string>("Enter the feature flag name");

switch (action)
{
    case Actions.AddFlag:
        serviceProvider.GetRequiredService<IFeatureFlagUpdater>().AddFlag(featureFlagName);
        break;

    case Actions.RemoveFlag:
        serviceProvider.GetRequiredService<IFeatureFlagUpdater>().RemoveFlag(featureFlagName);
        break;
}
