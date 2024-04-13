using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentCache<T> where T : Component
{
    private T cachedValue;

    private bool ShouldGet => cachedValue == null;

    public T Get(GameObject owner)
    {
        if (ShouldGet && !owner.TryGetComponent(out cachedValue))
        {
            Log.Debug($"No component of type {typeof(T)} found!");
        }
        return cachedValue;
    }

    public T Get(Component owner) { return Get(owner.gameObject); }
}

public class ChildrenCache<T> where T : Component
{
    private T[] cachedValues;

    private bool ShouldGet => cachedValues == null || cachedValues.Length == 0 || cachedValues.Any(e => e == null);

    public T[] Get(GameObject owner)
    {
        if (ShouldGet)
        {
            cachedValues = owner.GetComponentsInChildren<T>();
            if (ShouldGet) Log.Debug($"No components of type {typeof(T)} found!");
        }
        return cachedValues;
    }

    public T[] Get(Component owner) { return Get(owner.gameObject); }
}