using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public HexGridData<Piece> grid;
    public Vector2Int pos;
    public PieceType type;
    public bool team;
    public PieceBuff buffs;
    public bool movedThisTurn;
    public bool spawnedThisTurn;

    public bool HasBuff(PieceBuff buff) => (buffs & buff) > 0;

    public Piece(HexGridData<Piece> grid, Vector2Int pos, bool team, PieceType type, PieceBuff buffs, bool movedThisTurn, bool spawnedThisTurn)
    {
        this.grid = grid;
        this.pos = pos;
        this.type = type;
        this.team = team;
        this.buffs = buffs;
        this.movedThisTurn = movedThisTurn;
        this.spawnedThisTurn = spawnedThisTurn;
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
    None = 0, Taunt = 1, Shield = 2, Distract = 4, Charge = 8,
}
