using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayCardObject", menuName = "ScriptableObjects/PlayCardObject")]
public class PlayCardObject : ScriptableObject
{
    public PlayCardRegistry type;
    public PlayCardInstance prefab;
    public string title;
    public string description;

    public PlayCardInstance CreateInstance(Transform parent)
    {
        var instance = Instantiate(prefab, parent);
        instance.title.text = title;
        instance.description.text = description;
        return instance;
    }
}
