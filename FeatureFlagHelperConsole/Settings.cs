namespace FeatureFlagHelperConsole;

public record Settings
{
    public Settings(string enumPath, string jsonFilesDirectory)
    {
        EnumPath = enumPath;
        JsonFilePaths = Directory.GetFiles(jsonFilesDirectory, "*.*.json");
    }

    public string EnumPath { get; }

    public string[] JsonFilePaths { get; }
}
