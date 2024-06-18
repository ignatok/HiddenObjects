using UnityEngine;

namespace Services.Downloader
{
    public class GithubContentProvider : MonoBehaviour,IContentProvider
    {
        [SerializeField] private string configPath = "https://raw.githubusercontent.com/ignatok/Content/main/Content/json_config.txt";

        public string GetConfigPath()
        {
            return configPath;
        }
    }
}
