namespace FeatureFlagHelperConsole;

public record Settings
{
    public Settings(string enumPath, string jsonFilesDirectory, string ownedBy)
    {
        EnumPath = enumPath;
        OwnedBy = ownedBy;
        JsonFilePaths = Directory.GetFiles(jsonFilesDirectory, "*.*.json");
    }

    public string EnumPath { get; }

    public string OwnedBy { get; }

    public string[] JsonFilePaths { get; }
}
