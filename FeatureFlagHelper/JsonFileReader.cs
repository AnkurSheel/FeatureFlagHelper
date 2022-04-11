using System.Text.Json;
using System.Text.Json.Nodes;

namespace FeatureFlagHelper;

public interface IJsonFileReader
{
    JsonObject GetFlagsFromFile(string fileName);

    JsonObject GetFeatureFlagSection(JsonNode jsonNode);
}

public class JsonFileReader : IJsonFileReader
{
    public JsonFileReader()
    {
    }

    public JsonObject GetFlagsFromFile(string fileName)
    {
        var jsonString = File.ReadAllText(fileName);
        var jsonNode = JsonNode.Parse(jsonString, documentOptions:new JsonDocumentOptions{CommentHandling = JsonCommentHandling.Skip});

        if (jsonNode == null)
        {
            throw new ArgumentNullException(nameof(jsonNode), "Could not parse json");
        }

        return jsonNode.AsObject();
    }

    public  JsonObject GetFeatureFlagSection(JsonNode jsonNode)
    {
        var featureFlagsSection = jsonNode["FeatureFlags"];

        if (featureFlagsSection == null)
        {
            throw new ArgumentNullException(nameof(featureFlagsSection), "Could not read feature flags section");
        }

        return featureFlagsSection.AsObject();
    }
}
