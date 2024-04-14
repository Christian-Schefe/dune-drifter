using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInstance : MonoBehaviour
{
    private readonly ComponentCache<Shield> shield = new();
    private bool hasShield = false;
    private TweenRoutine tween;

    private void Awake()
    {
        if (!hasShield) LoseShield();
    }

    public void Move(Vector2Int from, Piece piece)
    {
        var main = Globals.Get<Main>();
        var grid = main.grid;
        var fromVec3 = grid.HexToWorld(from);
        var toVec3 = grid.HexToWorld(piece.pos);
        tween = Tween.Pos(transform, fromVec3, toVec3, 1.0f, Easing.Quad).Start(this, tween);
    }

    public void Attack(Vector2Int to, Piece piece)
    {
        var main = Globals.Get<Main>();
        var grid = main.grid;
        var fromVec3 = grid.HexToWorld(piece.pos);
        var toVec3 = grid.HexToWorld(to);

        var tween1 = Tween.Pos(transform, fromVec3, toVec3, 1.0f, Easing.Quad);
        var tween2 = tween1.Reversed();
        tween = tween1.Chain(tween2).Start(this, tween);
    }

    public void LoseShield()
    {
        shield.Get(this).LoseShield();
        hasShield = false;
    }

    public void GainShield()
    {
        shield.Get(this).GainShield();
        hasShield = true;
    }
}
