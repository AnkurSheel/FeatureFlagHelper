namespace FeatureFlagHelper;

public record Settings
{
    public Settings(string basePath)
    {
        EnumPath = $@"{basePath}\src\CDD.FeatureFlags/AvailableFlags.cs";
        JsonFolder = $@"{basePath}\src\Shared";
    }

    public string EnumPath { get; }

    public string JsonFolder { get;  }

}
