using Services.DI;
using UnityEngine;

namespace Services.SharedUtils
{
    public class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool isDontDestroyOnLoad;

        private bool IsDontDestroyOnLoad
        {
            get => isDontDestroyOnLoad;
            set => isDontDestroyOnLoad = value;
        }

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == false)
                    instance = FindObjectOfType<T>();
                if (instance == false)
                {
                    instance = new GameObject($"Singleton ({typeof(T).Name})").AddComponent<T>();
                    var bootStart = FindObjectOfType<BaseBootStart>();
                    if(bootStart)
                    {
                        instance.transform.SetParent(bootStart.MyTransform);
                        if(bootStart.IsDontDestroyOnLoad)
                            (instance as MonoSingleton<T>)!.IsDontDestroyOnLoad = bootStart.IsDontDestroyOnLoad;
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            var thisInstance = gameObject.GetComponent<T>();
            if(instance && instance != thisInstance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = thisInstance;
            if(IsDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}