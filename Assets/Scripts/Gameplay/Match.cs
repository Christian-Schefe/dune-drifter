using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParams
{
    public int arenaSize = 5;
    public int handSize = 5;
}

public class Match
{
    public Arena arena;
    public Hand<PlayCard> hand;
    public Run run;

    public Match(Run run, Deck<PlayCard> deck, MatchParams matchParams)
    {
        this.run = run;
        arena = new(matchParams.arenaSize);
        hand = new(deck, matchParams.handSize);
    }
}
