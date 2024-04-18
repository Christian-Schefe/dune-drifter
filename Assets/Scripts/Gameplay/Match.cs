using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParams
{
    public int arenaSize;
    public int handSize;
}

public class Match
{
    public Arena arena;
    public Hand hand;
    public Run run;
    public bool currentPlayer;

    public int CardsToPlay { get; private set; }

    public Match(Run run, Deck deck, MatchParams matchParams)
    {
        this.run = run;
        arena = new(matchParams.arenaSize);
        hand = new(deck, matchParams.handSize);
        currentPlayer = true;
        CardsToPlay = 1;
    }

    public void PlayCard(int cardIndex, Vector2Int target)
    {
        if (CardsToPlay <= 0) return;
        var card = hand.GetCard(cardIndex);
        if (ExecuteCard(card, target))
        {
            hand.PlayCard(card);
            CardsToPlay--;
        }
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        if (arena.CanMovePiece(from, to))
        {
            arena.MovePiece(from, to);
        }
    }

    public void EndTurn()
    {
        Main.Events.turnEnd();
        arena.OnEndTurn();
        currentPlayer = !currentPlayer;
        CardsToPlay = 1;
        Main.Events.turnStart();
    }

    private bool ExecuteCard(PlayCard card, Vector2Int target)
    {
        switch (card.type)
        {
            case PlayCardType.SpawnPiece:
                {
                    if (card.data is not (PieceType, bool)) return false;
                    var (type, team) = ((PieceType, bool))card.data;
                    if (!arena.CanSpawnPiece(target, team)) return false;
                    arena.SpawnPiece(target, type, team);
                    break;
                }
            case PlayCardType.GiveBuff:
                {
                    if (card.data is not PieceBuff buff) return false;
                    if (!arena.TryGetPiece(target, out _)) return false;
                    arena.GiveBuff(target, buff);
                    break;
                }
            case PlayCardType.Cleanse:
                {
                    if (card.data is not PieceBuff buff) return false;
                    if (!arena.TryGetPiece(target, out _)) return false;
                    arena.TakeBuff(target, ~PieceBuff.None);
                    break;
                }
        }

        return true;
    }
}
