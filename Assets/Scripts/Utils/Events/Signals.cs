using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISignal { }

public static class Signals
{
    private static readonly SignalHub hub = new();

    public static T Get<T>() where T : ISignal, new() => hub.Get<T>();
    public static ISignal Get(Type type) => hub.Get(type);
}

public class SignalHub
{
    private readonly Dictionary<Type, ISignal> signals = new();

    public T Get<T>() where T : ISignal, new()
    {
        Type signalType = typeof(T);
        if (!signals.ContainsKey(signalType)) signals.Add(signalType, new T());
        return (T)signals[signalType];
    }
    public ISignal Get(Type type)
    {
        if (!signals.ContainsKey(type)) signals.Add(type, Activator.CreateInstance(type) as ISignal);
        return signals[type];
    }
}

public abstract class Signal : ISignal
{
    private Action callback;

    public void AddListener(Action handler)
    {
#if UNITY_EDITOR
        Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
        callback += handler;
    }

    public void RemoveListener(Action handler) => callback -= handler;

    public void RemoveAllListeners() => callback = null;

    public void Dispatch() => callback?.Invoke();
}

public abstract class Signal<T> : ISignal
{
    private Action<T> callback;

    public void AddListener(Action<T> handler)
    {
#if UNITY_EDITOR
        Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
        callback += handler;
    }

    public void RemoveListener(Action<T> handler) => callback -= handler;

    public void RemoveAllListeners() => callback = null;

    public void Dispatch(T arg1) => callback?.Invoke(arg1);
}

public abstract class Signal<T, U> : ISignal
{
    private Action<T, U> callback;

    public void AddListener(Action<T, U> handler)
    {
#if UNITY_EDITOR
        Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
        callback += handler;
    }

    public void RemoveListener(Action<T, U> handler) => callback -= handler;

    public void RemoveAllListeners() => callback = null;

    public void Dispatch(T arg1, U arg2) => callback?.Invoke(arg1, arg2);
}

public abstract class Signal<T, U, V> : ISignal
{
    private Action<T, U, V> callback;

    public void AddListener(Action<T, U, V> handler)
    {
#if UNITY_EDITOR
        Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
        callback += handler;
    }

    public void RemoveListener(Action<T, U, V> handler) => callback -= handler;

    public void Dispatch(T arg1, U arg2, V arg3) => callback?.Invoke(arg1, arg2, arg3);
}
