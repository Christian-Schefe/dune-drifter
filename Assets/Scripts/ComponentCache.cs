using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentCache<T> where T : Component
{
    private T cachedValue;

    public T Get(GameObject owner)
    {
        if (cachedValue == null)
        {
            cachedValue = owner.GetComponent<T>();
        }
        return cachedValue;
    }
}
