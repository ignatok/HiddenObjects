using Cysharp.Threading.Tasks;
using Services.DI.Context;
using Services.Downloader;
using Services.GameData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private async void Start()
    {
        await DownloadConfig();
        SceneManager.LoadScene("MainMenu");
    }

    private async UniTask DownloadConfig()
    {
        var downloadService = ProjectContext.GetInstance<DownloadService>();
        var gameData = ProjectContext.GetInstance<GameData>();
        gameData.SetConfigData(await downloadService.StartDownloadConfig());
        
        if (gameData.Config == null)
        {
            Debug.LogWarning("not download config");    
        }
    }
}
