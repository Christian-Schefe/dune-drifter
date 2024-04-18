using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static T Get<T>()
    {
        var type = typeof(T);
        if (typeof(IQuery).IsAssignableFrom(type))
        {
            return (T)Queries.Get(type);
        }
        else if (typeof(ISignal).IsAssignableFrom(type))
        {
            return (T)Signals.Get(type);
        }
        else
        {
            return default;
        }
    }
}
