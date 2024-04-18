using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : GameObserver
{
    public PiecePrefabMapping mapping;

    public (Vector2Int, List<Vector2Int>)? clickedPiece;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CurrentPlayer)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        var arena = Globals.Get<ArenaManager>();
        arena.Deselect();

        var gridPos = Grid.RayIntersectRound(InputU.MouseRay);
        bool hasPiece = Arena.TryGetPiece(gridPos, out var piece);

        if (clickedPiece is (Vector2Int, List<Vector2Int>) p)
        {
            clickedPiece = null;
            if (p.Item1 == gridPos) return;
            else if (p.Item2.Contains(gridPos))
            {
                Match.MovePiece(p.Item1, gridPos);
                return;
            }
        }

        if (hasPiece)
        {
            var moveTargets = Arena.GetMoveTargets(piece);

            foreach (var pos in moveTargets) arena.Select(pos);

            clickedPiece = (gridPos, moveTargets);
        }
    }

    protected override void OnPieceSpawned(Piece piece, Vector2Int at)
    {
        var worldPos = Grid.HexToWorld(at);
        var prefab = mapping.Get<PieceInstance>(piece);
        var instance = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        instance.SetPiece(piece);
    }
}
