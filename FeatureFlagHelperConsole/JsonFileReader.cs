using System.Text.Json;
using System.Text.Json.Nodes;

namespace FeatureFlagHelperConsole;

public interface IJsonFileReader
{
    JsonObject GetRootJsonObject(string fileName);

    JsonObject GetFeatureFlagObject(JsonNode jsonNode);

    IReadOnlyCollection<string> GetFeatureFlags(string filename);
}

public class JsonFileReader : IJsonFileReader
{
    public JsonObject GetRootJsonObject(string fileName)
    {
        var jsonString = File.ReadAllText(fileName);
        var jsonNode = JsonNode.Parse(
            jsonString,
            documentOptions: new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            });

        if (jsonNode == null)
        {
            throw new ArgumentNullException(nameof(jsonNode), "Could not parse json");
        }

        return jsonNode.AsObject();
    }

    public JsonObject GetFeatureFlagObject(JsonNode jsonNode)
    {
        var featureFlagsSection = jsonNode["FeatureFlags"];

        if (featureFlagsSection == null)
        {
            throw new ArgumentNullException(nameof(featureFlagsSection), "Could not read feature flags section");
        }

        return featureFlagsSection.AsObject();
    }

    public IReadOnlyCollection<string> GetFeatureFlags(string filename)
    {
        var jsonObject = GetRootJsonObject(filename);
        var featureFlagObject = GetFeatureFlagObject(jsonObject);
        return featureFlagObject.Select(x => x.Key).ToList();
    }
}
