using System.Text;
using System.Text.Json.Nodes;

namespace FeatureFlagHelper;

public interface IFeatureFlagUpdater
{
    void RemoveFlag(string featureFlagName);

    void AddFlag(string featureFlagName);

    void UpdateFlag(string featureFlagName, IReadOnlyCollection<string> jsonFilePaths, bool enable);
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
        UpdateEnumFile(
            keys =>
            {
                if (!keys.Contains(featureFlagName))
                {
                    keys.Add(featureFlagName);
                }
            });

        UpdateJsons(_settings.JsonFilePaths,
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
        UpdateEnumFile(
            keys =>
            {
                keys.Remove(featureFlagName);
            });

        UpdateJsons(_settings.JsonFilePaths, featureFlagObject => featureFlagObject.Remove(featureFlagName));
    }

    public void UpdateFlag(string featureFlagName, IReadOnlyCollection<string> jsonFilePaths, bool enable)
    {
        UpdateJsons(jsonFilePaths,
            featureFlagObject =>
            {
                var featureFlagNode = featureFlagObject[featureFlagName];
                if (featureFlagNode == null)
                {
                    return;
                }

                featureFlagNode["enabled"] = enable;
            });
    }

    private void UpdateJsons(IReadOnlyCollection<string> jsonFilePaths, Action<JsonObject> updateFunc)
    {
        foreach (var file in jsonFilePaths)
        {
            var jsonObject = _jsonFileReader.GetRootJsonObject(file);
            var featureFlagObject = _jsonFileReader.GetFeatureFlagObject(jsonObject);

            updateFunc(featureFlagObject);

            _jsonFileWriter.WriteJsonFile(file, jsonObject);
        }
    }

    private void UpdateEnumFile(Action<List<string>> updateFunc)
    {
        var keys = _jsonFileReader.GetFeatureFlags(_settings.JsonFilePaths.First()).ToList();
        var allLines = File.ReadAllLines(_settings.EnumPath).Where(x => !keys.Contains(x.Trim().Replace(",", ""))).ToList();
        var index = allLines.Select(x => x.Trim()).ToList().IndexOf("}");

        var sb = new StringBuilder();

        for (var i = 0; i < index; i++)
        {
            sb.AppendLine(allLines[i]);
        }

        updateFunc(keys);

        for (var i = 0; i < keys.Count - 1; i++)
        {
            sb.AppendLine($"        {keys[i]},");
        }

        sb.AppendLine($"        {keys.Last()}");

        for (var i = index; i < allLines.Count; i++)
        {
            sb.AppendLine(allLines[i]);
        }

        File.WriteAllText(_settings.EnumPath, sb.ToString());
    }
}
