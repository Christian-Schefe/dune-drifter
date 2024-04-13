using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParams
{
    public int arenaSize = 5;
    public int handSize = 5;
    public HexGridData<Piece> grid;
}

public class Match
{
    public Arena arena;
    public Hand<PlayCard> hand;

    public Match(Deck<PlayCard> deck, MatchParams matchParams)
    {
        arena = new(matchParams.arenaSize);
        hand = new(deck, matchParams.handSize);
    }

    public void OnBeginMatch()
    {
        hand.OnBeginMatch();
        arena.OnBeginMatch(this);
    }
}
