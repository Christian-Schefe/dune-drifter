using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public PiecePrefabMapping mapping;

    public Dictionary<Vector2Int, PieceInstance> pieces = new();

    private void OnMatchStart(Match match)
    {

    }

    private void OnMatchEnd(Match match)
    {
        foreach (var piece in pieces)
        {
            Destroy(piece.Value.gameObject);
        }
        pieces.Clear();
    }

    private void OnSpawned(Vector2Int pos, Piece piece)
    {
        var main = Globals.Get<Main>();
        var worldPos = main.grid.HexToWorld(pos);
        var prefab = mapping.Get<PieceInstance>(piece);
        var instance = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        pieces.Add(pos, instance);

        Log.Info("Spawned Piece:", piece.pos, piece.type, piece.team);
    }

    private void OnMoved(Vector2Int pos, Piece piece)
    {
        var instance = pieces[pos];
        instance.Move(pos, piece);
    }

    private void OnAttacked(Vector2Int pos, Piece piece)
    {
        var instance = pieces[pos];
        instance.Attack(pos, piece);
    }

    private void OnLostShield(Vector2Int pos, Piece piece)
    {
        var instance = pieces[pos];
        instance.LoseShield();
    }

    private void OnEnable()
    {
        Signals.Get<PieceSignal.Spawned>().AddListener(OnSpawned);
        Signals.Get<PieceSignal.Moved>().AddListener(OnMoved);
        Signals.Get<PieceSignal.Attacked>().AddListener(OnAttacked);
        Signals.Get<PieceSignal.LostShield>().AddListener(OnLostShield);
        Signals.Get<MatchSignal.Start>().AddListener(OnMatchStart);
        Signals.Get<MatchSignal.End>().AddListener(OnMatchEnd);
    }

    private void OnDisable()
    {
        Signals.Get<PieceSignal.Spawned>().RemoveListener(OnSpawned);
        Signals.Get<PieceSignal.Moved>().RemoveListener(OnMoved);
        Signals.Get<PieceSignal.Attacked>().RemoveListener(OnAttacked);
        Signals.Get<PieceSignal.LostShield>().RemoveListener(OnLostShield);
        Signals.Get<MatchSignal.Start>().RemoveListener(OnMatchStart);
        Signals.Get<MatchSignal.End>().RemoveListener(OnMatchEnd);
    }
}
