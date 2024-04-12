using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Globals>();
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private static Globals _instance;

    private readonly Dictionary<Type, object> globals = new();

    private void Awake()
    {
        if (_instance == null) _instance = this;
        DontDestroyOnLoad(_instance.gameObject);
    }

    public static T Get<T>() where T : UnityEngine.Object
    {
        var type = typeof(T);
        var instance = Instance;
        if (instance.globals.TryGetValue(type, out var obj))
        {
            return (T)obj;
        }
        Log.Debug("Finding object of type: ", type);
        var foundObj = FindObjectOfType<T>();
        instance.globals.Add(type, foundObj);
        return foundObj;
    }

    public static T Get<T>(Func<T> factory)
    {
        var type = typeof(T);
        var instance = Instance;
        if (instance.globals.TryGetValue(type, out var obj))
        {
            return (T)obj;
        }
        Log.Debug("Creating object of type: ", type);
        var foundObj = factory();
        instance.globals.Add(type, foundObj);
        return foundObj;
    }
}
