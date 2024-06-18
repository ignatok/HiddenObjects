using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Services.DI.Context;
using Services.Downloader;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public interface ILevelContent
{
    event Action LevelDataLoaded;
    string Name { get; }
    string PreviewPathURL { get; }
    string PreviewLocalPath { get; }
    string RemoteDataPath { get;}
    string LocalDataPath { get;}
    int ItemsCount { get; }
    UniTask<Sprite> GetPreview();
    LevelData Data { get; }
    void StartLoadLevelData();
    void Unload();
}

public class LevelContent : ILevelContent
{
    public event Action LevelDataLoaded;
    private readonly string _remotePath;
    private readonly string _localPath;
    private readonly string _name;
    private readonly string _previewPath;
    private readonly string _previewLocalPath;
    private int _itemsCount;

    public bool IsLoaded => Data;
    
    public string LocalDataPath => _localPath;
    public string RemoteDataPath => _remotePath;

    public int ItemsCount => _itemsCount;

    public string Name => _name;

    public bool IsInternalBundle => _localPath?.Contains(Application.streamingAssetsPath) ?? false;
    
    public LevelData Data { get; private set; }

    public string PreviewPathURL => _previewPath;

    public string PreviewLocalPath => _previewLocalPath;

    public LevelContent(LevelConfig levelConfig)
    {
        _name = levelConfig.Name;
        _itemsCount = levelConfig.Counter;
        _remotePath = levelConfig.LevelDataURL;
        _localPath = Path.Combine(DataStorage.ContentDirectory , levelConfig.Name);
        _previewPath = levelConfig.ImageURL;
        _previewLocalPath = Path.Combine(DataStorage.PreviewDirectory , levelConfig.Name);
        Data = ScriptableObject.CreateInstance<LevelData>();
    }

    public UniTask<Sprite> GetPreview()
    {
        if (File.Exists(_previewLocalPath))
        {
            return LoadPreviewFromLocalStorage();
        }
       
        return LoadPreviewFromRemote();
    }

    private async UniTask<Sprite> LoadPreviewFromLocalStorage()
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("file://" + _previewLocalPath))
        {
            await webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var texture = DownloadHandlerTexture.GetContent(webRequest);
                return Sprite.Create(texture, new Rect(0,0, texture.width, texture.height),Vector2.one*0.5f);
            }
        }

        return null;
    }

    private async UniTask<Sprite> LoadPreviewFromRemote()
    {
        var downloadService = ProjectContext.GetInstance<DownloadService>();
        var texture = await downloadService.DownloadTexture(_previewPath);
        if (texture == null)
        {
            return null;
        }
        await File.WriteAllBytesAsync( _previewLocalPath, texture.EncodeToJPG());
        return Sprite.Create(texture, new Rect(0,0, texture.width, texture.height),Vector2.one*0.5f);
    }
    public void StartLoadLevelData()
    {
        
    }

    public void Unload()
    {
        Data = null;
    }
    
}