using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IQuery { }

public static class Queries
{
    private static readonly QueryHub hub = new();

    public static T Get<T>() where T : IQuery, new() => hub.Get<T>();
    public static IQuery Get(Type type) => hub.Get(type);
}

public class QueryHub
{
    private readonly Dictionary<Type, IQuery> queries = new();

    public T Get<T>() where T : IQuery, new()
    {
        Type queryType = typeof(T);
        if (!queries.ContainsKey(queryType)) queries.Add(queryType, new T());
        return (T)queries[queryType];
    }
    public IQuery Get(Type type)
    {
        if (!queries.ContainsKey(type)) queries.Add(type, Activator.CreateInstance(type) as IQuery);
        return queries[type];
    }
}

public abstract class Query<T> : IQuery
{
    private Func<T> callback;

    public void SetResponder(Func<T> handler) => callback = handler;

    public void RemoveResponder() => callback = null;

    public T Dispatch() => callback != null ? callback() : default;

    public bool Dispatch(out T result)
    {
        var hasHandler = callback != null;
        result = hasHandler ? callback() : default;
        return hasHandler;
    }
}

public abstract class Query<T, U> : IQuery
{
    private Func<T, U> callback;

    public void SetResponder(Func<T, U> handler) => callback = handler;

    public void RemoveResponder() => callback = null;

    public U Dispatch(T arg1) => callback != null ? callback(arg1) : default;

    public bool Dispatch(T arg1, out U result)
    {
        var hasHandler = callback != null;
        result = hasHandler ? callback(arg1) : default;
        return hasHandler;
    }
}
