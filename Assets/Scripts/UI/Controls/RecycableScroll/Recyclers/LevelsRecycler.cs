using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RecycableScroll;
using UI.Controls;
using UI.MainScreen;

public class LevelsRecycler : IContentRecycler
{
    private readonly List<Level> _data;
    private List<Element> _savedElements = new List<Element>();

    public int Count { get; private set; }

    private int _head;
    private int _tail;

    public int CurrentHead => _head;

    public bool LastItemReached => _tail >= Count;
    public bool FirstItemReached => _head < 0;

    public LevelsRecycler(List<Level> data)
    {
        _data = data;
        Count = data.Count;
        _head = -1;
        _tail = 0;
    }

    public LevelsRecycler(List<Level> data, int startIndex)
    {
        _data = data;
        Count = data.Count;
        _head = Mathf.Clamp(startIndex, -1, Count - 1);
        _tail = Mathf.Clamp(startIndex + 1, 0, Count);
    }

    public void Init(int chunkSize)
    {
    }

    public IElement GetItem(ItemPosition position, ContainerData container)
    {
        var id = 0;

        switch (position)
        {
            case ItemPosition.Head:
            {
                if (FirstItemReached)
                    return null;

                id = _head;
                _head--;
                break;
            }
            case ItemPosition.Tail:
            {
                if (LastItemReached)
                    return null;

                id = _tail;
                _tail++;
                break;
            }
        }

        var item = _savedElements.FirstOrDefault(e => e is LevelButton);

        if (!item)
        {
            // item = Pool.Get<LevelButton>();
            // item.RectTransform.SetTopAnchor();
            // item.Transform.SetParent(container.Transform, false);
            item.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.CellSize.x);
            item.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, container.CellSize.y);
        }
        else
        {
            item.gameObject.SetActive(true);
            _savedElements.Remove(item);
        }

        var levelButton = (LevelButton) item;
        levelButton.SetData(_data[id]);

        return item;
    }

    public Vector2 GetItemSize(ItemPosition position, ContainerData container)
    {
        return container.CellSize;
    }

    public void ReleaseItem(IElement item, ItemPosition position)
    {
        var element = (Element) item;
        ((IReleasable) element).OnRelease();

        element.gameObject.SetActive(false);
        _savedElements.Add(element);

        switch (position)
        {
            case ItemPosition.Head:
                _head++;
                break;
            case ItemPosition.Tail:
                _tail--;
                break;
        }
    }

    public void Clear()
    {
        _head = 0;
        _tail = 0;

        foreach (var element in _savedElements)
        {
            element.gameObject.SetActive(true);
            //Pool.Release(element);
        }
    }

    public void OnLevelProcessed(List<Level> levels)
    {
        foreach (var level in levels)
            _data.Add(level);

        Count = _data.Count;
    }
}

public interface IElement
{
    RectTransform RectTransform { get; }
    Transform Transform { get; }
}