using System;
using UnityEngine;

[Serializable]
public class HiddenItem
{
    [SerializeField] private string id;
    [SerializeField] private string group;
    [SerializeField] private string order;
    [SerializeField] private Sprite spriteLevel;
    [SerializeField] private Sprite spriteUi;
    [SerializeField] private Vector2 coords;
    [SerializeField] private Vector2 offset;


    public Sprite LevelSprite
    {
        get => spriteLevel;
        set => spriteLevel = value;
    }

    public Sprite UiSprite
    {
        get => spriteUi;
        set => spriteUi = value;
    }

    public Vector2 Coords
    {
        get => coords;
        set => coords = value;
    }

    public Vector2 Offset
    {
        get => offset;
        set => offset = value;
    }

    public Vector2 Position => Coords + Offset;

    public string Id
    {
        get => id;
        set => id = value;
    }

    public string Order
    {
        get => order;
        set => order = value;
    }
    
    public string Group
    {
        get => group;
        set => group = value;
    }

    public HiddenItem(string id, string group, string order, Sprite gameplaySprite, Sprite uiSprite, Vector2 position)
    {
        this.id = id;
        this.order = order;
        this.group = group;
        spriteLevel = gameplaySprite;
        spriteUi = uiSprite;
        coords = position;
    }
}