using System.Collections;
using System.Collections.Generic;
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
    }

    public void OnBeginMatch(Match match)
    {

    }

    public bool CanMovePiece(Vector2Int from, Vector2Int to)
    {
        if (grid.data[from] == null) return false;
        if (grid.data[to] != null) return false;

        return grid.data[from].CanMove(from, to);
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        var piece = grid.data[from];
        grid.data[to] = piece;
        grid.data[from] = null;
        piece.SetPosition(to);
    }
}

public class Piece
{
    public HexGridData<Piece> grid;
    public Vector2Int pos;
    public PieceType type;
    public bool team;

    public Piece(HexGridData<Piece> grid, Vector2Int pos, bool team, PieceType type)
    {
        this.grid = grid;
        this.pos = pos;
        this.type = type;
        this.team = team;
    }

    public void SetPosition(Vector2Int pos)
    {
        this.pos = pos;
    }

    public bool CanAttack(Vector2Int from, Vector2Int to)
    {
        return type switch
        {
            PieceType.Protector => false,
            _ => CanMove(from, to),
        };
    }

    public bool CanMove(Vector2Int from, Vector2Int to)
    {
        var dist = HexGrid.ManhattenDistance(from, to);
        var diff = to - from;

        return type switch
        {
            PieceType.Sphere => dist == 1,
            PieceType.Protector => team ? diff == HexGrid.Forward : diff == -HexGrid.Forward,
            _ => false,
        };
    }
}

public enum PieceType
{
    Sphere, Protector
}

public enum PieceBuffs
{
    Taunt, Shield, Distract
}