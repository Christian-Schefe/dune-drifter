using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayCard
{
    bool Execute(Match match, PlayCardTarget target);
}

public abstract class PlayCard<T> : IPlayCard where T : PlayCardTarget
{
    public abstract bool Execute(Match match, T target);

    public bool Execute(Match match, PlayCardTarget target)
    {
        return Execute(match, (T)target);
    }
}

public class SpawnPieceCard : PlayCard<PlayCardTarget.SingleFreeField>
{
    public PieceType piece;

    public SpawnPieceCard(PieceType piece)
    {
        this.piece = piece;
    }

    public override bool Execute(Match match, PlayCardTarget.SingleFreeField target)
    {
        if (!match.arena.CanSpawnPiece(target.target)) return false;
        match.arena.SpawnPiece(target.target, piece);
        return true;
    }
}

public abstract class PlayCardTarget
{
    public class SingleFreeField : PlayCardTarget
    {
        public Vector2Int target;
        public SingleFreeField(Vector2Int target)
        {
            this.target = target;
        }
    }
    public class OpponentPiece : PlayCardTarget
    {
        public Vector2Int target;
        public OpponentPiece(Vector2Int target)
        {
            this.target = target;
        }
    }
    public class PlayerPiece : PlayCardTarget
    {
        public Vector2Int target;
        public PlayerPiece(Vector2Int target)
        {
            this.target = target;
        }
    }
    public class None : PlayCardTarget { }
}