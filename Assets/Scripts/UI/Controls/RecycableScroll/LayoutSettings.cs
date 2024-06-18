using System;
using UnityEngine;

namespace RecycableScroll
{
    [Serializable]
    public class LayoutSettings
    {
        public Padding Padding;
        public int Columns;
        public Vector2 CellSize;
    }
    
    [Serializable]
    public struct Padding
    {
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;

        public Vector2 Space;
    }
}