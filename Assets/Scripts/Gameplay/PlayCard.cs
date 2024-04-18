using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard
{
    public PlayCardType type;
    public int? handPosition;
    public object data;

    public PlayCard(PlayCardType type, object data)
    {
        this.type = type;
        handPosition = null;
        this.data = data;
    }
}

public enum PlayCardType
{
    SpawnPiece, GiveBuff, Cleanse
}
