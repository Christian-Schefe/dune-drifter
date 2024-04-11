using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton]
public class SaveState : MonoBehaviour
{
    private void Awake()
    {
        Log.Debug("hello");
        Log.Info("hello");
        Log.Warn("hello");
        Log.Err("hello");
    }
}
