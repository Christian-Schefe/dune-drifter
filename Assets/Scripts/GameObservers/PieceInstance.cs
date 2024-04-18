using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInstance : GameObserver
{
    private readonly ComponentCache<Shield> shield = new();
    private TweenRoutine tween;

    private Piece shadow;

    public void SetPiece(Piece piece)
    {
        shadow = piece;
        UpdateShield();
    }

    protected override void OnPieceMoved(Piece piece, Vector2Int from, Vector2Int to)
    {
        if (piece != shadow) return;

        var fromVec3 = Grid.HexToWorld(from);
        var toVec3 = Grid.HexToWorld(to);

        this.TweenPosition().From(fromVec3).To(toVec3).Duration(0.3f).Ease(Easing.QuadInOut).RunImmediate(ref tween);
    }

    protected override void OnPieceAttacked(Piece piece, Vector2Int from, Vector2Int to)
    {
        if (piece != shadow) return;

        var fromVec3 = Grid.HexToWorld(from);
        var toVec3 = Grid.HexToWorld(to);

        this.TweenPosition().From(fromVec3).To(toVec3).Duration(0.3f).PingPong(2).Ease(Easing.QuadInOut).RunImmediate(ref tween);
    }

    protected override void OnPieceLostShield(Piece piece, Vector2Int at)
    {
        if (piece != shadow) return;
        UpdateShield();
    }

    protected override void OnPieceDied(Piece piece, Vector2Int at)
    {
        if (piece != shadow) return;
        this.TweenScale().To(Vector3.zero).Duration(0.3f).Ease(Easing.QuadInOut).OnFinally(() => Destroy(gameObject)).RunImmediate(ref tween);
    }

    protected override void OnMatchEnd()
    {
        OnPieceDied(shadow, shadow.pos);
    }

    private void UpdateShield()
    {
        bool isOn = shadow.HasBuff(PieceBuff.Shield);
        if (isOn) shield.Get(this).GainShield();
        else shield.Get(this).LoseShield();
    }
}
