using System.Text;
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
        UpdateEnumFile(
            keys =>
            {
                if (!keys.Contains(featureFlagName))
                {
                    keys.Add(featureFlagName);
                }

                return keys;
            });

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
        UpdateEnumFile(
            keys =>
            {
                if (keys.Contains(featureFlagName))
                {
                    keys.Remove(featureFlagName);
                }

                return keys;
            });

        UpdateJsons(featureFlagObject => featureFlagObject.Remove(featureFlagName));
    }

    private void UpdateJsons(Action<JsonObject> updateFunc)
    {
        foreach (var file in _settings.JsonFilePaths)
        {
            var jsonObject = _jsonFileReader.GetFlagsFromFile(file);
            var featureFlagObject = _jsonFileReader.GetFeatureFlagSection(jsonObject);

            updateFunc(featureFlagObject);

            _jsonFileWriter.WriteJsonFile(file, jsonObject);
        }
    }

    private void UpdateEnumFile(Func<List<string>, List<string>> updateFunc)
    {
        var jsonObject = _jsonFileReader.GetFlagsFromFile(_settings.JsonFilePaths.First());
        var featureFlagObject = _jsonFileReader.GetFeatureFlagSection(jsonObject);

        var keys = featureFlagObject.Select(x => x.Key).ToList();

        var allLines = File.ReadAllLines(_settings.EnumPath).Where(x => !keys.Contains(x.Trim().Replace(",", ""))).ToList();
        var index = allLines.Select(x => x.Trim()).ToList().IndexOf("}");

        var sb = new StringBuilder();

        for (var i = 0; i < index; i++)
        {
            sb.AppendLine(allLines[i]);
        }

        keys = updateFunc(keys);

        for (var i = 0; i < keys.Count - 1; i++)
        {
            sb.AppendLine($"        {keys[i]},");
        }
        sb.AppendLine($"        {keys.Last()}");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        File.WriteAllText(_settings.EnumPath, sb.ToString());
    }
}
