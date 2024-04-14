using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run
{
    public List<RuleCard> ruleCards;
    public Deck<IPlayCard> deck;

    public LoopState currentLoopState;

    public Match currentMatch;
    public MatchParams currentMatchParams;

    public Run()
    {
        ruleCards = new List<RuleCard>();
        currentLoopState = LoopState.Match;
        currentMatchParams = new() { arenaSize = 6, handSize = 5 };
        SetDefaultDeck();
    }

    public void SetDefaultDeck()
    {
        List<IPlayCard> playCards = new();
        for (int i = 0; i < 10; i++)
        {
            playCards.Add(new SpawnPieceCard(PieceType.Sphere));
        }
        deck = new(playCards);
    }

    public void BeginMatch()
    {
        currentLoopState = LoopState.Match;
        currentMatch = new(this, deck, currentMatchParams);

        Log.Info("Match Started");
        Signals.Get<MatchSignal.Start>().Dispatch(currentMatch);
    }

    public void EndMatch()
    {
        Log.Info("Match Ended");
        Signals.Get<MatchSignal.End>().Dispatch(currentMatch);

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