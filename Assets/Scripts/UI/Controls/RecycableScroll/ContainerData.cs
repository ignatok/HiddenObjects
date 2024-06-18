using UnityEngine;

namespace RecycableScroll
{
    public class ContainerData
    {
        public readonly RectTransform Transform;
        public readonly Vector2 CellSize;

        public ContainerData(RectTransform transform, Vector2 cellSize)
        {
            Transform = transform;
            CellSize = cellSize;
        }
    }
}