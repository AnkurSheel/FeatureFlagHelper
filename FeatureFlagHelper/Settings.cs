namespace FeatureFlagHelper;

public record Settings
{
    public Settings(string basePath)
    {
        EnumPath = $@"{basePath}\src\CDD.FeatureFlags/AvailableFlags.cs";
        JsonFilePaths = Directory.GetFiles($@"{basePath}\src\Shared", "*.*.json");
    }

    public string EnumPath { get; }

    public string[] JsonFilePaths { get; }
}
