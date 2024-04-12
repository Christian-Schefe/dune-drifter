using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Log
{
    public static void Debug(params object[] message)
    {
        LogMessage("DEBUG", new Color(0.5f, 0.5f, 1.0f), message);
    }

    public static void Info(params object[] message)
    {
        LogMessage("INFO", Color.green, message);
    }

    public static void Warn(params object[] message)
    {
        LogMessage("WARN", Color.yellow, message);
    }

    public static void Err(params object[] message)
    {
        LogMessage("ERROR", Color.red, message);
    }

    private static void LogMessage(string prefix, Color color, object[] objects)
    {
#if UNITY_EDITOR
        var objStr = string.Join(" ", objects.Select(e => ObjToString(e)));
        var str = $"[<color=#{ColorUtility.ToHtmlStringRGB(color)}>{prefix}</color>] {objStr}";
        UnityEngine.Debug.Log(str);
#endif
    }

    public static string ObjToString(object obj, bool explicitString = false)
    {
        return obj switch
        {
            string stri => explicitString ? $"\"{stri}\"" : stri,
            Object unityObj => $"{unityObj.GetType()}(\"{unityObj.name}\")",
            DictionaryEntry entry => $"{ObjToString(entry.Key)}: {ObjToString(entry.Value)}",
            System.Type type => $"Type({type})",
            IDictionary dict => $"{{{IterToStr(dict)}}}",
            IEnumerable iter => $"[{IterToStr(iter)}]",
            object any => any.ToString()
        };
    }

    private static string IterToStr(IEnumerable iter, string sep = ", ")
    {
        return string.Join(sep, iter.Cast<object>().Select(e => ObjToString(e, true)));
    }
}
