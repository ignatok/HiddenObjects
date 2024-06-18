using Services.DI.Container;
using UnityEngine;

namespace Services.DI
{
    public abstract class BaseBootStart: MonoBehaviour, IContainerMember
    {
        [SerializeField] private bool isDontDestroyOnLoad = true;
        public bool IsDontDestroyOnLoad => isDontDestroyOnLoad;
        public Transform MyTransform { get; protected set; }
        public abstract void Dispose();
        public abstract void Initialization();
    }
}