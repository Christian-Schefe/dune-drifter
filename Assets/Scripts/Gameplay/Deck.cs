using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck<T>
{
    public List<T> cards;

    public Stack<T> remainingCards;

    public Deck(List<T> cards)
    {
        this.cards = cards;
        remainingCards = new(cards.Shuffled());
    }

    public bool DrawCard(out T card)
    {
        var canDraw = remainingCards.Count > 0;
        card = canDraw ? remainingCards.Pop() : default;
        return canDraw;
    }
}
public class Hand<T>
{
    public Deck<T> deck;
    public List<T> drawnCards;
    public int handSize;

    public Hand(Deck<T> deck, int handSize)
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
            if (!deck.DrawCard(out T card)) break;
            drawnCards.Add(card);
        }
    }

    public List<T> PlayCards(List<int> cardIndices)
    {
        cardIndices.Sort((a, b) => b.CompareTo(a));
        List<T> cards = new();
        foreach (int i in cardIndices)
        {
            cards.Add(drawnCards[i]);
            drawnCards.RemoveAt(i);
        }
        DrawCards();
        return cards;
    }
}
