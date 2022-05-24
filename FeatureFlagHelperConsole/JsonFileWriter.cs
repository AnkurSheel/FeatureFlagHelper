using System.Text.Json;
using System.Text.Json.Nodes;

namespace FeatureFlagHelperConsole;

public interface IJsonFileWriter
{
    void WriteJsonFile(string filePath, JsonObject jsonObject);
}

public class JsonFileWriter : IJsonFileWriter
{
    public void WriteJsonFile(string filePath, JsonObject jsonObject)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        File.WriteAllText(filePath, jsonObject.ToJsonString(options));
    }
}
