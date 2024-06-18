using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string Name;
    
    [SerializeField] private Sprite background;
    [SerializeField] private List<HiddenItem> items = new List<HiddenItem>();
    
    public Sprite Background
    {
        get => background;
        set => background = value;
    }

    public List<HiddenItem> Items
    {
        get => items;
        set => items = value;
    }

    public static async Task<LevelData> ParseFrom(AssetBundle bundle)
    {
        if (bundle == null)
            return null;

        try
        {
            var r = bundle.LoadAssetAsync<LevelData>("Bundle.asset");

            await UniTask.WaitWhile(() => !r.isDone);

            return (LevelData) r.asset;
        }
        catch
        {
            return null;
        }
    }
}