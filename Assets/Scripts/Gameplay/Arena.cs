using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arena
{
    public int size;
    public HexGridData<Piece> grid;

    public Arena(int size)
    {
        this.size = size;
        grid = new();
        grid.SetHexagonShape(size);

        Queries.Get<GetMoveOptionsQuery>().SetResponder(pos =>
        {
            var piece = grid.data[pos];
            if (piece == null) return new();
            var moveTargets = piece.GetMoveTargets().Where(e => CanMovePiece(pos, e)).ToList();
            var attackTargets = piece.GetAttackTargets().Where(e => CanAttackPiece(pos, e)).ToList();
            return (moveTargets, attackTargets);
        });
    }

    public bool CanSpawnPiece(Vector2Int at)
    {
        return grid.data[at] == null;
    }

    public bool CanAttackPiece(Vector2Int from, Vector2Int to)
    {
        if (grid.data[from] == null) return false;
        var piece = grid.data[from];

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
        var moveTargets = piece.GetMoveTargets();
        return moveTargets.Contains(to);
    }

    public void SpawnPiece(Vector2Int at, Piece piece)
    {
        grid.data[at] = piece;
        Signals.Get<PieceSpawnedResponse>().Dispatch(at, piece);
    }

    public void AttackPiece(Vector2Int from, Vector2Int to)
    {
        var piece = grid.data[from];
        var targetPiece = grid.data[to];
        if (targetPiece.buffs.HasFlag(PieceBuff.Shield))
        {
            targetPiece.buffs &= ~PieceBuff.Shield;
            Signals.Get<PieceLostShieldResponse>().Dispatch(to, targetPiece);
        }
        else
        {
            grid.data[to] = null;
        }

        Signals.Get<PieceAttackedResponse>().Dispatch(to, piece);
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        var piece = grid.data[from];
        grid.data[to] = piece;
        grid.data[from] = null;
        piece.SetPosition(to);

        Signals.Get<PieceMovedResponse>().Dispatch(from, piece);
    }
}

public class Piece
{
    public HexGridData<Piece> grid;
    public Vector2Int pos;
    public PieceType type;
    public bool team;
    public PieceBuff buffs;

    public Piece(HexGridData<Piece> grid, Vector2Int pos, bool team, PieceType type, PieceBuff buffs)
    {
        this.grid = grid;
        this.pos = pos;
        this.type = type;
        this.team = team;
        this.buffs = buffs;
    }

    public void SetPosition(Vector2Int pos)
    {
        this.pos = pos;
    }

    public List<Vector2Int> GetAttackTargets()
    {
        var list = type switch
        {
            PieceType.Protector => new(),
            _ => GetMoveTargets(),
        };

        return list;
    }

    public List<Vector2Int> GetMoveTargets()
    {
        var list = type switch
        {
            PieceType.Sphere => grid.neighbours[pos],
            PieceType.Protector => new() { pos + HexGrid.Forward },
            _ => new(),
        };

        return list;
    }
}

public enum PieceType
{
    Sphere, Protector
}

[System.Flags]
public enum PieceBuff
{
    None = 0, Taunt = 1, Shield = 2, Distract = 4,
}