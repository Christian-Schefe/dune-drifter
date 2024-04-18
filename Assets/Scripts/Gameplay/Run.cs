using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run
{
    public List<RuleCard> ruleCards;
    public Deck deck;

    public LoopState currentLoopState;

    public Match currentMatch;
    public MatchParams currentMatchParams;

    public Run()
    {
        ruleCards = new List<RuleCard>();
        currentLoopState = LoopState.Match;
        currentMatchParams = new() { arenaSize = 5, handSize = 5 };
        SetDefaultDeck();
    }

    private void SetDefaultDeck()
    {
        List<PlayCard> playCards = new();
        for (int i = 0; i < 10; i++)
        {
            playCards.Add(new PlayCard(PlayCardType.SpawnPiece, (PieceType.Protector, true)));
        }
        deck = new(playCards);
    }

    public void BeginMatch()
    {
        currentLoopState = LoopState.Match;
        currentMatch = new(this, deck, currentMatchParams);

        Main.Events.matchStart();
    }

    public void EndMatch()
    {
        Main.Events.matchEnd();

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
