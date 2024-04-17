using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Deck
{
    public List<IPlayCard> cards;

    public Stack<IPlayCard> remainingCards;

    public Deck(List<IPlayCard> cards)
    {
        this.cards = cards;
        remainingCards = new(cards.Shuffled());
    }

    public bool DrawCard(out IPlayCard card)
    {
        var canDraw = remainingCards.Count > 0;
        card = canDraw ? remainingCards.Pop() : default;
        return canDraw;
    }
}
public class Hand
{
    public Deck deck;
    public List<IPlayCard> drawnCards;
    public int handSize;

    public Hand(Deck deck, int handSize)
    {
        this.deck = deck;
        this.handSize = handSize;
        drawnCards = new();
        DrawCards();
    }

    public void DrawCards()
    {
        int cardsToDraw = handSize - drawnCards.Count;
        for (int i = 0; i < cardsToDraw; i++)
        {
            if (!deck.DrawCard(out IPlayCard card)) break;
            drawnCards.Add(card);
            Events.Get<HandSignal.Draw>().Dispatch(drawnCards.Count - 1, this);
        }
    }

    public IPlayCard GetCard(int i)
    {
        return drawnCards[i];
    }

    public IPlayCard PlayCard(int i)
    {
        IPlayCard card = drawnCards[i];
        drawnCards.RemoveAt(i);
        Events.Get<HandSignal.Play>().Dispatch(i, this);
        DrawCards();
        return card;
    }

    public List<IPlayCard> PlayCards(List<int> cardIndices)
    {
        cardIndices.Sort((a, b) => b.CompareTo(a));
        List<IPlayCard> cards = new();
        foreach (int i in cardIndices)
        {
            cards.Add(drawnCards[i]);
            drawnCards.RemoveAt(i);
        }
        DrawCards();
        return cards;
    }
}
