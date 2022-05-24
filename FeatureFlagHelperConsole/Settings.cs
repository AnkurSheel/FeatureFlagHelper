namespace FeatureFlagHelperConsole;

public record Settings
{
    public Settings(string enumPath, string jsonFilesDirectory, string ownedBy)
    {
        if (string.IsNullOrWhiteSpace(enumPath) || string.IsNullOrWhiteSpace(jsonFilesDirectory) || string.IsNullOrWhiteSpace(ownedBy))
        {
            throw new NullReferenceException("Properties not set in appSettings.json");
        }
        
        EnumPath = enumPath;
        OwnedBy = ownedBy;
        JsonFilePaths = Directory.GetFiles(jsonFilesDirectory, "*.*.json");
    }

    public string EnumPath { get; }

    public string OwnedBy { get; }

    public string[] JsonFilePaths { get; }
}
