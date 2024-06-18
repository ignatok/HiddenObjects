using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class DelayCall
{
    public static void RunAsync(TimeSpan delay, Action action)
    {
        Task.Run(async () =>
        {
            await Task.Delay(delay);
            await UniTask.SwitchToMainThread();
            action?.Invoke();
        });
    }
    
    public static Coroutine Run(float delay, Action action)
    {
        return MainThreadHelper.StartCoroutineFromMainThread(DelayRoutine(delay, action));
    }

    private static IEnumerator DelayRoutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
    
    public static Coroutine SkipFrames(int frames, Action action)
    {
        return MainThreadHelper.StartCoroutineFromMainThread(DelayRoutine(frames, action));
    }

    private static IEnumerator DelayRoutine(int frames, Action action)
    {
        for (var i = 0; i < frames; i++)
            yield return null;
        
        action?.Invoke();
    }

    public static void Stop(Coroutine coroutine)
    {
        if(coroutine != null)
            MainThreadHelper.StopCoroutineFromMainThread(coroutine);
    }
}