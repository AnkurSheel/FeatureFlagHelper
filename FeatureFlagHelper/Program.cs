// See https://aka.ms/new-console-template for more information

using FeatureFlagHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

var settings = new Settings(configuration["enumPath"], configuration["jsonFilesDirectory"]);

var serviceProvider = ServiceRegistry.RegisterServices(new ServiceCollection(), settings).BuildServiceProvider();

var action = AnsiConsole.Prompt(
    new SelectionPrompt<string>().Title("What do you want to do?").AddChoices(Actions.AddFlag, Actions.RemoveFlag, Actions.EnableFlag, Actions.DisableFlag));

var featureFlagUpdater = serviceProvider.GetRequiredService<IFeatureFlagUpdater>();
var jsonFileReader = serviceProvider.GetRequiredService<IJsonFileReader>();

string? featureFlagName;

switch (action)
{
    case Actions.AddFlag:
        featureFlagName = AnsiConsole.Ask<string>("Enter the feature flag name");

        featureFlagUpdater.AddFlag(featureFlagName);
        break;

    case Actions.RemoveFlag:
        featureFlagName = AnsiConsole.Ask<string>("Enter the feature flag name");

        featureFlagUpdater.RemoveFlag(featureFlagName);
        break;

    case Actions.EnableFlag:
    {
        var keys = jsonFileReader.GetFeatureFlags(settings.JsonFilePaths.First()).ToList();
        featureFlagName = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Which flag do you want to enable?").AddChoices(keys));

        var jsonFiles = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>().Title("Which environments do you want to update?").AddChoices(settings.JsonFilePaths));
        featureFlagUpdater.UpdateFlag(featureFlagName, jsonFiles, true);
        break;
    }

    case Actions.DisableFlag:
    {
        var keys = jsonFileReader.GetFeatureFlags(settings.JsonFilePaths.First()).ToList();
        featureFlagName = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Which flag do you want to enable?").AddChoices(keys));

        var jsonFiles = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>().Title("Which environments do you want to update?").AddChoices(settings.JsonFilePaths));
        featureFlagUpdater.UpdateFlag(featureFlagName, jsonFiles, false);
        break;
    }
}

AnsiConsole.MarkupLine($"[green]Updated files[/]");
