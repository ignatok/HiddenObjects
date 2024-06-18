using System.IO;
using System.Text;
using NaughtyAttributes;
using UnityEngine;

namespace Services.GameData
{
    public class ConfigCreator : MonoBehaviour
    {
        [Label("path to json for loading data")] [SerializeField]
        private string loadingJsonPath;

        [Header("DATA")] [SerializeField] private PackConfig packConfig;

        [Button("generate json")]
        public void GenerateJson()
        {
            var json = JsonUtility.ToJson(packConfig, true);
            Debug.Log(json);
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "json_config.txt"), json);
        }

        [Button("load data from json")]
        public void LoadFromJson()
        {
            var text = File.ReadAllText(loadingJsonPath, Encoding.Default);
            packConfig = JsonUtility.FromJson<PackConfig>(text);
        }
    }
}