using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class InAppGrid : GridLayoutGroup
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;

        protected override void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void CalculateCellsWidth()
        {
            var width = _rectTransform.rect.width;
            var cellsCount = 2;

            if ((float)Screen.height / Screen.width <= 1.4f)
                cellsCount = 3;

            var cellsAspect = cellSize.x / cellSize.y;
            var cellWidth = width / cellsCount - (cellsCount - 1) * spacing.x / cellsCount;
            var cellHeight = cellWidth / cellsAspect;

            cellSize = new Vector2(cellWidth, cellHeight);
        }

        public float GetHeight()
        {
            var width = _rectTransform.rect.size.x;

            var cellsInRow = Mathf.Max(1,
                Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));

            var childCount = transform.childCount;
            var rows = childCount / cellsInRow;

            if (childCount % cellsInRow > 0)
                rows++;

            var height = cellSize.y * rows + spacing.y * (rows - 1);

            return height;
        }
    }
}