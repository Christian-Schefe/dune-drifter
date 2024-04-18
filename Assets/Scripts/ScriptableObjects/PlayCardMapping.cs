using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayCardMapping", menuName = "ScriptableObjects/PlayCardMapping")]
public class PlayCardMapping : ScriptableObject
{
    public PlayCardObject[] mappings;

    private Dictionary<PlayCardType, PlayCardObject> typeMap;

    public Dictionary<PlayCardType, PlayCardObject> GetDict()
    {
        if (typeMap != null && typeMap.Count > 0) return typeMap;
        typeMap ??= new();

        foreach (var entry in mappings)
        {
            typeMap[entry.type] = entry;
        }
        return typeMap;
    }

    public PlayCardObject Get(PlayCardType type) => GetDict()[type];
}
