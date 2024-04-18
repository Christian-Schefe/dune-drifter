using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public Deck deck;
    public PlayCard[] drawnCards;
    public int handSize;

    private readonly Queue<int> indexQueue;

    public Hand(Deck deck, int handSize)
    {
        this.deck = deck;
        this.handSize = handSize;

        indexQueue = new();
        for (int i = 0; i < handSize; i++) indexQueue.Enqueue(i);

        drawnCards = new PlayCard[handSize];
        DrawCards();
    }

    public void DrawCards()
    {
        int cardsToDraw = indexQueue.Count;
        for (int i = 0; i < cardsToDraw; i++)
        {
            if (!deck.DrawCard(out PlayCard card)) break;
            card.handPosition = indexQueue.Dequeue();
            drawnCards[card.handPosition.Value] = card;

            Main.Events.playCardDrawn(card);
        }
    }

    public PlayCard GetCard(int i)
    {
        return drawnCards[i];
    }

    public void PlayCard(PlayCard card)
    {
        int index = card.handPosition.Value;
        drawnCards[index] = null;
        indexQueue.Enqueue(index);
        Main.Events.playCardPlayed(card);
        DrawCards();
    }
}
