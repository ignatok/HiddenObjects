using UnityEngine;

namespace Services.Downloader
{
    public class GoogleDiskContentProvider : MonoBehaviour, IContentProvider
    {
        [SerializeField] private string configPath =
            "https://drive.google.com/uc?export=download&id=1-N9gvW_fSB0P7C_SxY0c4D2-j1HOmDiB";

        public string GetConfigPath()
        {
            return configPath;
        }
    }
}