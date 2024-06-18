using System.IO;
using Cysharp.Threading.Tasks;

public class Level
{
    private readonly LevelContent _resources;
    private readonly LevelProgress _progress;
    public ILevelContent Resources => _resources;
    public ILevelProgress Progress => _progress;

    private UniTaskCompletionSource<LoadResult> _resourceLoadingSource;

    public Level(LevelConfig config)
    {
        _resources = new LevelContent(config);
        _progress = new LevelProgress(Path.Combine(DataStorage.SaveDirectory, config.Name));
    }
    
    public void Unload()
    {
        _resources?.Unload();
    }

    public void DeleteResourcesWithoutProgress()
    {
        _resources.Delete();
    }

    public void ClearLoadedResources()
    {
        if (_progress.IsComplete)
            _resources.Delete();
    }
    
    public void Restart()
    {
        _progress.Reset();
    }
}