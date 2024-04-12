using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveState : MonoBehaviour
{
    public Dictionary<string, string> serializedState = new();
    public Dictionary<string, System.Func<string>> stateRetrievers = new();
    public bool hasLoaded = false;
    public string fileName = "saveState.json";

    private void Awake()
    {
        Load();
    }

    [ContextMenu("Delete Save")]
    private void DeleteSave()
    {
        JsonManager.Delete(fileName);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Save();
    }

    public static T Use<T>(string key, System.Func<T> retriever)
    {
        var self = Globals.Get<SaveState>();

        if (!self.stateRetrievers.ContainsKey(key))
        {
            self.stateRetrievers.Add(key, () => JsonUtility.ToJson(retriever()));
        }

        if (self.GetValue(key, out T val))
        {
            return val;
        }
        else
        {
            var obj = retriever();
            self.serializedState.Add(key, JsonUtility.ToJson(obj));
            return obj;
        }
    }

    private bool GetValue<T>(string key, out T val)
    {
        Load();
        var result = serializedState.TryGetValue(key, out var str);
        val = result ? JsonUtility.FromJson<T>(str) : default;
        return result;
    }

    private void Load()
    {
        if (!hasLoaded) serializedState = JsonManager.CreateOrLoad(fileName, SaveStateData.Default()).ToDict();
    }

    private void Save()
    {
        foreach (var pair in stateRetrievers)
        {
            serializedState[pair.Key] = pair.Value();
        }
        JsonManager.Save(fileName, SaveStateData.FromDict(serializedState));
    }
}

[System.Serializable]
public struct SaveStateData
{
    public List<string> keys;
    public List<string> values;

    public static SaveStateData FromDict(Dictionary<string, string> dict)
    {
        var list = dict.Select(e => (e.Key, e.Value));
        return new SaveStateData
        {
            keys = list.Select(e => e.Key).ToList(),
            values = list.Select(e => e.Value).ToList()
        };
    }

    public readonly Dictionary<string, string> ToDict()
    {
        var list = keys.Zip(values, (a, b) => (a, b));
        return list.ToDictionary(e => e.a, e => e.b);
    }

    public static SaveStateData Default()
    {
        return new SaveStateData
        {
            keys = new(),
            values = new(),
        };
    }
}

public static class JsonManager
{
    public static void Save<T>(string fileName, T data)
    {
        var filePath = Path.Join(Application.persistentDataPath, fileName);

        try
        {
            string jsonString = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonString);
            Log.Info($"Saved file at \"{filePath}\": {jsonString}");
        }
        catch (System.Exception e)
        {
            Log.Err($"Couldn't read the file at \"{filePath}\": {e}");
        }
    }

    public static T Load<T>(string fileName)
    {
        var filePath = Path.Join(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                Log.Info($"Loaded file at \"{filePath}\": {jsonString}");
                return JsonUtility.FromJson<T>(jsonString);
            }
            catch (System.Exception e)
            {
                Log.Err($"Couldn't read the file at \"{filePath}\": {e}");
                return default;
            }
        }
        else
        {
            Log.Err($"Couldn't find file at \"{filePath}\"");
            return default;
        }
    }

    public static void CreateAndSave<T>(string fileName, T data)
    {
        var filePath = Path.Join(Application.persistentDataPath, fileName);
        try
        {
            string jsonString = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonString);
            Log.Info($"Saved file at \"{filePath}\"");
        }
        catch (System.Exception e)
        {
            Log.Err($"Couldn't read the file at \"{filePath}\": {e}");
        }
    }

    public static T CreateOrLoad<T>(string fileName, T defaultContents)
    {
        var filePath = Path.Join(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) Save(fileName, defaultContents);
        return Load<T>(fileName);
    }

    public static void Delete(string fileName)
    {
        var filePath = Path.Join(Application.persistentDataPath, fileName);
        File.Delete(filePath);
        Log.Info($"Deleted file at \"{filePath}\"");
    }
}