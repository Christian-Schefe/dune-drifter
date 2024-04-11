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
        DontDestroyOnLoad(gameObject);
        if (_instance == null) _instance = this;

        var typesWithAttribute = AttributeFinder.GetClassesWithAttribute<SingletonAttribute>();
        Log.Debug("Found types with attribute: ", typesWithAttribute);
        foreach (var type in typesWithAttribute)
        {
            globals.Add(type, FindObjectOfType(type));
        }
        Log.Debug("Current globals: ", globals);
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

public class SingletonAttribute : Attribute
{

}

public static class AttributeFinder
{
    public static Type[] GetClassesWithAttribute<T>() where T : Attribute
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var classesWithAttribute = types.Where(type => type.GetCustomAttribute<T>() != null).ToArray();

        return classesWithAttribute;
    }
}