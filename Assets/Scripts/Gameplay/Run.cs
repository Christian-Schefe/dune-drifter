using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run
{
    public List<RuleCard> ruleCards;
    public Deck<PlayCard> deck;

    public LoopState currentLoopState;

    public Match currentMatch;
    public MatchParams currentMatchParams;

    public Run()
    {
        ruleCards = new List<RuleCard>();
        currentLoopState = LoopState.Match;
    }

    public void BeginMatch()
    {
        currentMatch = new(deck, currentMatchParams);
    }
}

public enum LoopState
{
    Match, Shop
}