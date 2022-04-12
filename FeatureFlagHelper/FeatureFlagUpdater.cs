using System.Text.Json.Nodes;

namespace FeatureFlagHelper;

public interface IFeatureFlagUpdater
{
    void RemoveFlag(string featureFlagName);

    void AddFlag(string featureFlagName);
}

public class FeatureFlagUpdater : IFeatureFlagUpdater
{
    private readonly Settings _settings;
    private readonly IJsonFileReader _jsonFileReader;
    private readonly IJsonFileWriter _jsonFileWriter;

    public FeatureFlagUpdater(Settings settings, IJsonFileReader jsonFileReader, IJsonFileWriter jsonFileWriter)
    {
        _settings = settings;
        _jsonFileReader = jsonFileReader;
        _jsonFileWriter = jsonFileWriter;
    }

    public void AddFlag(string featureFlagName)
    {
        UpdateJsons(
            featureFlagObject =>
            {
                if (featureFlagObject.ContainsKey(featureFlagName))
                {
                    return;
                }

                featureFlagObject[featureFlagName] = new JsonObject
                {
                    ["enabled"] = false
                };
            });
    }

    public void RemoveFlag(string featureFlagName)
    {
        UpdateJsons(featureFlagObject => featureFlagObject.Remove(featureFlagName));
    }

    private void UpdateJsons(Action<JsonObject> updateFunc)
    {
        var jsonFiles = Directory.GetFiles(_settings.JsonFolder, "*.*.json");

        foreach (var file in jsonFiles)
        {
            var jsonObject = _jsonFileReader.GetFlagsFromFile(file);
            var featureFlagObject = _jsonFileReader.GetFeatureFlagSection(jsonObject);

            updateFunc(featureFlagObject);

            _jsonFileWriter.WriteJsonFile(file, jsonObject);
        }
    }
}
