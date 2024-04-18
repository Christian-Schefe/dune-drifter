using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PiecePrefabMappingEntry
{
    public PieceType pieceType;
    public GameObject playerPrefab;
    public GameObject opponentPrefab;
}

[CreateAssetMenu(fileName = "PiecePrefabMapping", menuName = "ScriptableObjects/PiecePrefabMapping")]
public class PiecePrefabMapping : ScriptableObject
{
    public PiecePrefabMappingEntry[] mappings;

    private Dictionary<PieceType, GameObject> playerPrefabs;
    private Dictionary<PieceType, GameObject> opponentPrefabs;

    public Dictionary<PieceType, GameObject> GetDict(bool team)
    {
        ref var dict = ref team ? ref playerPrefabs : ref opponentPrefabs;
        if (dict != null && dict.Count > 0) return dict;
        dict ??= new();

        foreach (var entry in mappings)
        {
            dict[entry.pieceType] = team ? entry.playerPrefab : entry.opponentPrefab;
        }
        return dict;
    }

    public GameObject Get(Piece piece) => GetDict(piece.team)[piece.type];
    public T Get<T>(Piece piece) where T : Component => Get(piece).GetComponent<T>();

    public Dictionary<PieceType, GameObject> PlayerPrefabs => GetDict(true);

    public Dictionary<PieceType, GameObject> OpponentPrefabs => GetDict(true);
}
