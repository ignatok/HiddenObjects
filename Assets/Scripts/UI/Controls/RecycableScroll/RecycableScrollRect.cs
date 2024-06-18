using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RecycableScroll
{
    public class RecycableScrollRect : ScrollRect
    {
        [SerializeField] private LayoutSettings _layoutSettings;
        [SerializeField] private RectTransform _viewBorders;

        private const float Epsilon = 0.1f;

        private readonly List<IElement> _items = new List<IElement>();
        private ContainerData _containerData;
        private RectTransform _rectTransform;
        private Vector2 _prevContainerPosition;
        private IContentRecycler _recycler;

        private bool _isDragging;

        //private Rect Rect => _viewBorders.GetWorldRect();

        private bool Inited => IsActive() && _recycler != null;

        private float ContentHeight => _items.Count == 0
            ? 0
            : _items[0].RectTransform.anchoredPosition.y
              - _items[_items.Count - 1].RectTransform.anchoredPosition.y
              + _items[_items.Count - 1].RectTransform.rect.height;

        public bool IsDragging => _isDragging;

        public event Action<Vector2> Dragged;

        public int CalculateColumnsCount()
        {
            var width = _rectTransform.rect.width
                        - _layoutSettings.Padding.Left
                        - _layoutSettings.Padding.Right;

            return (int) Mathf.Round(width / _layoutSettings.CellSize.x);
        }

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();

            content.anchorMin = new Vector2(0.5f, 1f);
            content.anchorMax = new Vector2(0.5f, 1f);
            content.pivot = new Vector2(0.5f, 1);
        }

        public void Init(IContentRecycler recycler)
        {
            Clear();
            StopMovement();

            _recycler = null;

            if (recycler == null)
                return;

            var width = _rectTransform.rect.width
                        - _layoutSettings.Padding.Left
                        - _layoutSettings.Padding.Right;

            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _rectTransform.rect.height);

            m_ContentStartPosition = Vector2.zero;
            content.anchoredPosition = Vector2.zero;

            _layoutSettings.Columns = (int) Mathf.Round(width / _layoutSettings.CellSize.x);

            width -= _layoutSettings.Padding.Space.x * (_layoutSettings.Columns - 1);

            var cellWidth = width / _layoutSettings.Columns;
            var cellHeight = cellWidth * _layoutSettings.CellSize.y / _layoutSettings.CellSize.x;
            _layoutSettings.CellSize = new Vector2(cellWidth, cellHeight);

            _containerData = new ContainerData(content, _layoutSettings.CellSize);

            _viewBorders.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, viewport.rect.height);

            var position = _viewBorders.anchoredPosition;
            var sizeDelta = _viewBorders.sizeDelta;

            sizeDelta.y += _layoutSettings.CellSize.y * 3;
            position.y = _layoutSettings.CellSize.y;

            _viewBorders.sizeDelta = sizeDelta;
            _viewBorders.anchoredPosition = position;

            _recycler = recycler;
            _recycler.Init(_layoutSettings.Columns);

            if (_recycler.Count == 0)
                return;

            Fill();
            CleanupItems();
            UpdateContainer();
        }

        private void UpdateContainer()
        {
            var offset = 0f;

            if (_items.Count > 0)
            {
                var height = Mathf.Max(ContentHeight, _rectTransform.rect.height);
                content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

                offset = _items[0].RectTransform.anchoredPosition.y;

                var lastItemRect = _items[_items.Count - 1].RectTransform;
                var bottomOffset = height - (-lastItemRect.anchoredPosition.y + lastItemRect.rect.height);

                if (bottomOffset < 0)
                    offset = bottomOffset;
            }

            if (offset == 0)
                return;

            foreach (var item in _items)
            {
                var pos = item.RectTransform.anchoredPosition;
                pos.y -= offset;
                item.RectTransform.anchoredPosition = pos;
            }

            var containerPos = content.anchoredPosition;
            containerPos.y += offset;
            _prevContainerPosition.y += offset;

            SetContentAnchoredPosition(_prevContainerPosition);
            UpdateBounds();
            UpdatePrevData();
            SetContentAnchoredPosition(containerPos);
        }

        private Vector2 GetItemPositionPrev(Vector2 itemSize)
        {
            if (_items.Count == 0)
                return Vector2.zero;

            var lastItem = _items[0];
            var pos = lastItem.RectTransform.anchoredPosition;

            pos.x -= _layoutSettings.Padding.Space.x + itemSize.x;

            if (pos.x + _layoutSettings.Padding.Space.x < 0)
            {
                pos.x = content.rect.width - itemSize.x;
                pos.y += _layoutSettings.Padding.Space.y + itemSize.y;
            }

            return pos;
        }

        private Vector2 GetItemPositionNext(Vector2 itemSize)
        {
            if (_items.Count == 0)
                return Vector2.zero;

            var lastItem = _items[_items.Count - 1];
            var pos = lastItem.RectTransform.anchoredPosition;

            pos.x += lastItem.RectTransform.rect.width + _layoutSettings.Padding.Space.x;

            if (pos.x - _layoutSettings.Padding.Space.x + itemSize.x > content.rect.width)
            {
                pos.x = 0;
                pos.y -= _layoutSettings.Padding.Space.y + lastItem.RectTransform.rect.height;
            }

            return pos;
        }

        private void CleanupItems()
        {
            // while (!_recycler.LastItemReached && _items.Count > 0 && _items[0].WorldRect.yMin > Rect.yMax)
            // {
            //     _recycler.ReleaseItem(_items[0], ItemPosition.Head);
            //     _items.RemoveAt(0);
            // }
            //
            // while (!_recycler.FirstItemReached && _items.Count > 0 &&
            //        _items[_items.Count - 1].WorldRect.yMax < Rect.yMin)
            // {
            //     _recycler.ReleaseItem(_items[_items.Count - 1], ItemPosition.Tail);
            //     _items.RemoveAt(_items.Count - 1);
            // }

            UpdateContainer();
        }

        private void Fill()
        {
            while (ContentHeight < _viewBorders.rect.height)
            {
                if (!AddBottomLine())
                    break;
            }

            if (content.anchoredPosition.y < _viewBorders.anchoredPosition.y)
                AddTopLine();

            if (content.anchoredPosition.y - content.rect.height >
                _viewBorders.anchoredPosition.y - _viewBorders.rect.height)
                AddBottomLine();
        }

        private bool AddBottomLine()
        {
            var baseY = _viewBorders.rect.yMin - _layoutSettings.Padding.Space.y;
            var size = _recycler.GetItemSize(ItemPosition.Tail, _containerData);
            var pos = GetItemPositionNext(size);

            while (!_recycler.LastItemReached && pos.y >= baseY)
            {
                var item = _recycler.GetItem(ItemPosition.Tail, _containerData);

                if (item == null)
                    break;

                item.Transform.SetAsLastSibling();
                item.RectTransform.anchoredPosition = pos;
                _items.Add(item);

                size = _recycler.GetItemSize(ItemPosition.Tail, _containerData);
                pos = GetItemPositionNext(size);
            }

            UpdateContainer();
            return !_recycler.LastItemReached;
        }

        private void AddTopLine()
        {
            var baseY = _layoutSettings.Padding.Space.y;
            var size = _recycler.GetItemSize(ItemPosition.Head, _containerData);
            var pos = GetItemPositionPrev(size);

            while (!_recycler.FirstItemReached && pos.y - size.y <= baseY + Epsilon)
            {
                var item = _recycler.GetItem(ItemPosition.Head, _containerData);

                if (item == null)
                    break;

                item.Transform.SetAsFirstSibling();
                item.RectTransform.anchoredPosition = pos;
                _items.Insert(0, item);

                size = _recycler.GetItemSize(ItemPosition.Head, _containerData);
                pos = GetItemPositionPrev(size);
            }

            UpdateContainer();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            var containerPos = content.anchoredPosition;
            containerPos += eventData.delta;
            Dragged?.Invoke(eventData.delta);

            SetContentAnchoredPosition(containerPos);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            base.OnEndDrag(eventData);
        }

        private void Update()
        {
            if (!Inited)
                return;

            if (Time.frameCount % 3 == 0)
                Fill();
        }

        protected override void LateUpdate()
        {
            if (!Inited)
                return;

            if (Time.frameCount % 3 == 0)
            {
                UpdateContainer();
                CleanupItems();
            }

            base.LateUpdate();

            CleanupItems();
            _prevContainerPosition = content.anchoredPosition;
        }

        public void Clear()
        {
            foreach (var item in _items)
                _recycler.ReleaseItem(item, ItemPosition.Head);

            _items.Clear();
            _recycler?.Clear();

            _recycler = null;
        }
    }

    public enum ItemPosition
    {
        Head,
        Tail
    }
}