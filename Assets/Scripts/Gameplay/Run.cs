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
        currentLoopState = LoopState.Match;
        currentMatch = new(this, deck, currentMatchParams);

        Log.Info("Match Started");
        Signals.Get<MatchStartEvent>().Dispatch(currentMatch);
    }

    public void EndMatch()
    {
        Log.Info("Match Ended");
        Signals.Get<MatchEndEvent>().Dispatch(currentMatch);

        currentMatch = null;
        BeginShop();
    }

    public void BeginShop()
    {
        currentLoopState = LoopState.Shop;
    }

    public void EndShop()
    {
        BeginMatch();
    }
}

public enum LoopState
{
    Match, Shop
}