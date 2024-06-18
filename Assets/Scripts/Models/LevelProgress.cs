using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public interface ILevelProgress
{
    bool this[string key] { get; set; }
    bool IsExist { get; }
    bool IsLoaded { get; }
    bool IsComplete { get; set; }
    int FoundedCount { get; }
    int Counter { get; set; }

    void Preload();
    void Save();
    string GetFirstNotFoundedItem();
    string GetFirstNotFoundedItem(string group);
    void AddElapsedTime(TimeSpan time);
}

public class LevelProgress : ILevelProgress
{
    private readonly string _path;
    private LevelProgressData _data;
    private int _counter;
    private bool _isComplete;

    public event Action Inited;
    public event Action Changed;
    public event Action Resetted;

    public int ContentVersion => _data?.ContentVersion ?? 0;
    public int FoundedCount { get; private set; } = -1;
    public bool IsLoaded => _data != null;

    public bool IsComplete
    {
        get => _data.IsComplete;
        set => _data.IsComplete = value;
    }

    public bool IsInvalid => _data == null;
    public bool IsExist => File.Exists(_path);

    public DateTime DateStarted { get; private set; }
    public DateTime DateUpdated { get; private set; }

    public DateTime DateCompleted { get; private set; }

    public TimeSpan ElapsedTime => _data.ElapsedTime;


    public int Counter
    {
        get => _data.Counter;
        set => _data.Counter = value;
    }

    public bool this[string key]
    {
        get => _data.Map[key];
        set
        {
            DateUpdated = DateTime.Now;

            _data.Map[key] = value;

            FoundedCount = _data.Map.Count(x => x.Value);
            IsComplete = _counter <= 0;

            if (IsComplete)
                DateCompleted = DateTime.Now;

            Save();

            Changed?.Invoke();
        }
    }

    public LevelProgress(string savePath)
    {
        _path = savePath;
        _data = new LevelProgressData(new List<HiddenItem>(), 1);
    }

    public void Preload()
    {
        Load();
    }

    public void Load()
    {
        if (File.Exists(_path))
        {
            var readTask = File.ReadAllText(_path);
            _data = JsonUtility.FromJson<LevelProgressData>(readTask);
        }
    }

    public void Save()
    {
        if (!IsExist)
        {
            File.Create(_path).Close();
            File.WriteAllText(_path, JsonUtility.ToJson(_data));
        }
        else
        {
            File.WriteAllText(_path, JsonUtility.ToJson(_data));
        }
    }

    public void Reset()
    {
        IsComplete = false;
        Save();
    }

    public void UpdateVersion(int contentVersion)
    {
        if (_data == null)
            return;

        _data.ContentVersion = contentVersion;
        Save();
    }

    public string GetFirstNotFoundedItem()
    {
        foreach (var (key, value) in _data.Map)
            if (!value)
                return key;

        return string.Empty;
    }

    public string GetFirstNotFoundedItem(string group)
    {
        foreach (var (key, value) in _data.Map)
            if (key.Split('.')[0] == group && !value)
                return key;

        return string.Empty;
    }

    public void AddElapsedTime(TimeSpan time)
    {
        _data.ElapsedTime += time;
        Save();
    }

    public void Fill()
    {
        var keys = _data.Map.Keys.ToList();

        foreach (var key in keys)
            _data.Map[key] = true;
    }

    public void FillOne()
    {
        var keys = _data.Map.Keys.ToList();
        _data.Map[keys[0]] = true;
    }
}