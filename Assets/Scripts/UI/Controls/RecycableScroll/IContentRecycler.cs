using System.Collections.Generic;
using UnityEngine;

namespace RecycableScroll
{
    public interface IContentRecycler
    {
        bool LastItemReached { get; }
        bool FirstItemReached { get; }
        int CurrentHead { get; }
        int Count { get; }
        void Init(int chunkSize);
        IElement GetItem(ItemPosition position, ContainerData container);
        Vector2 GetItemSize(ItemPosition position, ContainerData container);
        void ReleaseItem(IElement item, ItemPosition position);
        void Clear();
        public void OnLevelProcessed(List<Level> levels);
    }
}