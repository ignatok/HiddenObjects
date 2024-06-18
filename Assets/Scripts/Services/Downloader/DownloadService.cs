using Cysharp.Threading.Tasks;
using Services.DI.Container;
using UnityEngine;
using UnityEngine.Networking;
using File = System.IO.File;

namespace Services.Downloader
{
    public class DownloadService : IContainerMember
    {
        private readonly string _configPath;

        public DownloadService(IContentProvider contentProvider)
        {
            _configPath = contentProvider.GetConfigPath();
        }

        public async UniTask<PackConfig> StartDownloadConfig( bool saveToLocal = false)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_configPath))
            {
                await webRequest.SendWebRequest();
                
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(webRequest.downloadHandler.text);
                    PackConfig data = JsonUtility.FromJson<PackConfig>(webRequest.downloadHandler.text);
                    if (saveToLocal)
                    {
                        await File.WriteAllBytesAsync(DataStorage.ContentConfigPath, webRequest.downloadHandler.data);
                    }
                    return data;
                }
            }

            return null;
        }

        public async UniTask<Texture2D> DownloadTexture(string path)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(path, false))
            {
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var content = DownloadHandlerTexture.GetContent(webRequest);
                    return content;
                }
                else if (webRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Initialization()
        {
        }

        public void Dispose()
        {
        }
    }
}