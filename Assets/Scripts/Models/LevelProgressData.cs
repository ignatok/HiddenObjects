using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelProgressData
{
    public Dictionary<string, bool> Map = new Dictionary<string, bool>();
    public int ContentVersion;
    public TimeSpan ElapsedTime;
    public int Counter;
    public bool IsComplete;
    public LevelProgressData(IEnumerable<HiddenItem> data, int contentVersion)
    {
        foreach (var item in data)
            Map[item.Id] = false;
        
        ElapsedTime = TimeSpan.Zero;
        ContentVersion = contentVersion;
    }

    public void Reset()
    {
        var keys = Map.Keys.ToList();
        foreach (var key in keys)
            Map[key] = false;
        
        ElapsedTime = TimeSpan.Zero;
    }

    public void Complete()
    {
        var keys = Map.Keys.ToList();
        foreach (var key in keys)
            Map[key] = true;
        
        ElapsedTime = new TimeSpan(1000);
    }


}