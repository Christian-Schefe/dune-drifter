using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arena
{
    public int size;
    public HexGridData<Piece> grid;

    private List<Piece> movedThisTurn = new();
    private List<Piece> spawnedThisTurn = new();

    public Arena(int size)
    {
        this.size = size;
        grid = new();
        grid.SetHexagonShape(size);

        SpawnDefaultPieces();
    }

    public List<Vector2Int> GetMoveTargets(Piece piece)
    {
        var targets = piece.GetMoveTargets().Where(p => CanMovePiece(piece.pos, p));
        return targets.ToList();
    }

    public List<Vector2Int> GetAttackTargets(Piece piece)
    {
        var targets = piece.GetAttackTargets().Where(p => CanAttackPiece(piece.pos, p));
        return targets.ToList();
    }

    public void OnEndTurn()
    {
        foreach (var piece in movedThisTurn) piece.movedThisTurn = false;
        foreach (var piece in spawnedThisTurn) piece.spawnedThisTurn = false;
        movedThisTurn.Clear();
        spawnedThisTurn.Clear();
    }

    public bool TryGetPiece(Vector2Int at, out Piece piece) => grid.data.TryGetValue(at, out piece) && piece != null;

    private void SpawnDefaultPieces()
    {
        Vector2Int p1 = new(0, 1 - size);
        Vector2Int p2 = new(0, size - 1);
        SpawnPiece(p1, PieceType.Sphere, true);
        SpawnPiece(p2, PieceType.Sphere, false);
        grid.data[p1].movedThisTurn = false;
        grid.data[p1].spawnedThisTurn = false;
        grid.data[p2].movedThisTurn = false;
        grid.data[p2].spawnedThisTurn = false;
    }

    public bool CanSpawnPiece(Vector2Int at, bool team)
    {
        if (!grid.Inside(at) || grid.data[at] != null) return false;

        foreach (var n in grid.neighbours[at])
        {
            if (grid.data[n] != null && grid.data[n].team == team) return true;
        }

        return false;
    }

    public bool CanAttackPiece(Vector2Int from, Vector2Int to)
    {
        if (grid.data[from] == null) return false;
        var piece = grid.data[from];
        if (piece.movedThisTurn) return false;
        if (piece.spawnedThisTurn && !piece.HasBuff(PieceBuff.Charge)) return false;

        if (grid.data[to] == null || grid.data[to].team == piece.team) return false;
        var targetPiece = grid.data[from];

        var attackTargets = piece.GetAttackTargets().Where(e => grid.data[e] != null && grid.data[e].team != piece.team).ToList();

        if (attackTargets.Count > 1 && targetPiece.buffs.HasFlag(PieceBuff.Distract))
        {
            return false;
        }
        if (attackTargets.Any(e => e != to && grid.data[e].buffs.HasFlag(PieceBuff.Taunt)))
        {
            return false;
        }

        return true;
    }

    public bool CanMovePiece(Vector2Int from, Vector2Int to)
    {
        if (grid.data[from] == null) return false;
        if (grid.data[to] != null) return false;

        var piece = grid.data[from];
        if (piece.movedThisTurn) return false;
        if (piece.spawnedThisTurn && !piece.HasBuff(PieceBuff.Charge)) return false;

        var moveTargets = piece.GetMoveTargets();
        return moveTargets.Contains(to);
    }

    public void GiveBuff(Vector2Int at, PieceBuff buff)
    {
        var piece = grid.data[at];
        piece.buffs |= buff;
    }

    public void TakeBuff(Vector2Int at, PieceBuff buff)
    {
        var piece = grid.data[at];
        piece.buffs &= ~buff;
    }

    public void SpawnPiece(Vector2Int at, PieceType type, bool team)
    {
        Piece piece = new(grid, at, team, type, PieceBuff.None, false, true);
        grid.data[at] = piece;
        Main.Events.pieceSpawned(piece, at);
        spawnedThisTurn.Add(piece);
    }

    public void AttackPiece(Vector2Int from, Vector2Int to)
    {
        var piece = grid.data[from];
        var targetPiece = grid.data[to];
        if (targetPiece.buffs.HasFlag(PieceBuff.Shield))
        {
            targetPiece.buffs &= ~PieceBuff.Shield;
            Main.Events.pieceLostShield(targetPiece, to);
        }
        else
        {
            grid.data[to] = null;
        }

        piece.movedThisTurn = true;
        movedThisTurn.Add(piece);
        Main.Events.pieceAttacked(piece, from, to);
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        var piece = grid.data[from];
        grid.data[to] = piece;
        grid.data[from] = null;
        piece.SetPosition(to);

        piece.movedThisTurn = true;
        movedThisTurn.Add(piece);
        Main.Events.pieceMoved(piece, from, to);
    }
}
