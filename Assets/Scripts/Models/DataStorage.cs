using System.IO;
using System.Linq;
using UnityEngine;

public static class DataStorage
{
    public static readonly string SaveDirectory;
    public static readonly string PreviewDirectory;
    public static readonly string ContentDirectory;
    public static readonly string ContentConfigPath;
    public static readonly string PlayerDataDirectory;

    static DataStorage()
    {
        SaveDirectory = Path.Combine(Application.persistentDataPath, "Data");
        PreviewDirectory = Path.Combine(Application.persistentDataPath, "Resources", "LevelsPreviews");
        ContentDirectory = Path.Combine(Application.persistentDataPath, "Content");
        PlayerDataDirectory = Path.Combine(Application.persistentDataPath, "PlayerData");
        ContentConfigPath = Path.Combine(Application.persistentDataPath, GameConfig.ConfigName);
        
        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        if (!Directory.Exists(PreviewDirectory))
            Directory.CreateDirectory(PreviewDirectory);

        if (!Directory.Exists(PlayerDataDirectory))
            Directory.CreateDirectory(PlayerDataDirectory);

        if (!Directory.Exists(ContentDirectory))
            Directory.CreateDirectory(ContentDirectory);
        
    }
}