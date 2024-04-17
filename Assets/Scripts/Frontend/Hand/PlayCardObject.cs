using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayCardObject", menuName = "ScriptableObjects/PlayCardObject")]
public class PlayCardObject : ScriptableObject
{
    public PlayCardRegistry type;
    public PlayCardInstance prefab;
    public string title;
    public Sprite image;

    public PlayCardInstance CreateInstance(Transform parent)
    {
        var instance = Instantiate(prefab, parent);
        instance.title.text = title;
        instance.image.sprite = image;
        return instance;
    }
}
