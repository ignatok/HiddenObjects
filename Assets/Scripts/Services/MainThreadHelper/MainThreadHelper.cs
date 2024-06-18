using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThreadHelper : MonoBehaviour
{
    public static event Action<bool> ApplicationFocusChanged;
    public static event Action<bool> ApplicationPauseStatusChanged;
    public static event Action ApplicationQuit;
    public static event Action ApplicationUpdate;
    public static event Action ApplicationOnGui;

    private static readonly object Mutex = new object();
    private static CancellationTokenSource _disposeTokenSource = new CancellationTokenSource();

    private static MainThreadHelper _instance;
    private static int _mainThreadId = -1;
    private static bool _isApplicationClosing;

    private readonly List<Action> _mainThreadActions = new List<Action>();
    private string _persistentDataPath;

    public static CancellationToken DisposeToken => _disposeTokenSource.Token;

    public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == _mainThreadId;

    public static string GetPersistentDataPath => Instance._persistentDataPath;

    private static MainThreadHelper Instance
    {
        get
        {
            if (_isApplicationClosing)
                return null;

            if (!_instance)
            {
                var helpers = FindObjectsOfType(typeof(MainThreadHelper)) as MainThreadHelper[];

                if (helpers != null && helpers.Length != 0)
                {
                    if (helpers.Length == 1)
                    {
                        _instance = helpers[0];
                        _instance.gameObject.name = nameof(MainThreadHelper);
                        DontDestroyOnLoad(_instance.gameObject);

                        return _instance;
                    }

                    Debug.LogError("You have more than one " + nameof(MainThreadHelper) +
                                   " in the scene. Sry, i'll kill them...'");

                    foreach (var helper in helpers)
                        Destroy(helper.gameObject);
                }

                var go = new GameObject(nameof(MainThreadHelper), typeof(MainThreadHelper));

                _instance = go.GetComponent<MainThreadHelper>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Instance._persistentDataPath = Application.persistentDataPath;
        _mainThreadId = Thread.CurrentThread.ManagedThreadId;
    }

    public static Coroutine StartCoroutineFromMainThread(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void StopCoroutineFromMainThread(Coroutine coroutine)
    {
        if (coroutine == null)
            return;

        Instance.StopCoroutine(coroutine);
    }

    public static void StopCoroutineFromMainThread(IEnumerator coroutine)
    {
        Instance.StopCoroutine(coroutine);
    }

    public static void Run(Action action)
    {
        if (Instance == null)
            return;

        lock (Mutex)
        {
            Instance._mainThreadActions.Add(action);
        }
    }

    public static T Create<T>(T obj) where T : MonoBehaviour
    {
        return _instance.CreateInternal(obj);
    }

    public static GameObject CreateEmpty(string name)
    {
        return _instance.CreateEmptyInternal(name);
    }

    public static void ToDoNotDestroy(GameObject obj)
    {
        Instance.ToDoNotDestroyInternal(obj);
    }

    private static void HandleActions()
    {
        if (Instance._mainThreadActions.Count <= 0)
            return;

        lock (Mutex)
        {
            for (var i = 0; i < Instance._mainThreadActions.Count; i++)
            {
                Instance._mainThreadActions[i]?.Invoke();
                Instance._mainThreadActions.RemoveAt(i);
                i--;
            }
        }
    }

    private T CreateInternal<T>(T obj) where T : MonoBehaviour
    {
        return Instantiate(obj);
    }

    private GameObject CreateEmptyInternal(string name)
    {
        return Instantiate(new GameObject(name));
    }

    private void ToDoNotDestroyInternal(GameObject obj)
    {
        DontDestroyOnLoad(obj);
    }

    private void OnEnable()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void OnGUI()
    {
        ApplicationOnGui?.Invoke();
    }

    private void Update()
    {
        HandleActions();
        ApplicationUpdate?.Invoke();
    }

    private void OnApplicationQuit()
    {
        _disposeTokenSource.Cancel();
    }

    public static void ResetApp()
    {
        _disposeTokenSource.Cancel();
        _disposeTokenSource = new CancellationTokenSource();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (Instance != this)
            return;

        ApplicationFocusChanged?.Invoke(hasFocus);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (Instance != this)
            return;

        ApplicationPauseStatusChanged?.Invoke(pauseStatus);
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        _isApplicationClosing = true;
        ApplicationQuit?.Invoke();
        _disposeTokenSource.Cancel();
    }

    private void Subscribe()
    {
        //SpotService.AddListener(SpotNames.Application.ApplicationFocusChanged, SpotService_OnApplicationFocusChanged);
    }

    private void UnSubscribe()
    {
        // SpotService.RemoveListener(SpotNames.Application.ApplicationFocusChanged,
        //     SpotService_OnApplicationFocusChanged);
    }

    // private void SpotService_OnApplicationFocusChanged(object[] obj)
    // {
    //     var focus = obj.GetParameterByIndex<bool>(1);
    //     OnApplicationFocus(focus);
    // }
}