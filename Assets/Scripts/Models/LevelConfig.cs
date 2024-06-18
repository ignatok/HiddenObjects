using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum LevelType
{
    Default,
    OneByOne,
    Same,
}

[Serializable]
public class LevelConfig 
{
    public int Id;
    public string ImageURL;
    public string Name;
    public int Counter;
    public string Category;
    public string LevelDataURL;
}

[Serializable]
public class PackConfig
{
    public List<LevelConfig> Levels;
    
    [JsonConstructor]
    public PackConfig(List<LevelConfig> levels)
    {
        Levels = levels;
    }
    public PackConfig()
    {
        Levels = new List<LevelConfig>();
    }
}



[Serializable]
public class GameConfig
{
    public const string ConfigName = "config.json";
    public const string TestConfigName = "config_test.json";
    
    public string MinAppVersion;
    public List<string> CategoryOrder;
    public PackConfig[] Packs;
}

public static class DataExtensions
{
    public static LevelConfig FindLevelById(this List<PackConfig> packConfigs, string id)
    {
        LevelConfig result = null;
        foreach (var packConfig in packConfigs)
        {
            foreach (var levelConfig in packConfig.Levels.Where(levelConfig => levelConfig.Id.Equals(id)))
            {
                result = levelConfig;
                break;
            }
        }

        return result;
    }
}