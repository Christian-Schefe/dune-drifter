using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Deck
{
    public List<PlayCard> cards;

    public Stack<PlayCard> remainingCards;

    public Deck(List<PlayCard> cards)
    {
        this.cards = cards;
        remainingCards = new(cards.Shuffled());
    }

    public bool DrawCard(out PlayCard card)
    {
        var canDraw = remainingCards.Count > 0;
        card = canDraw ? remainingCards.Pop() : default;
        return canDraw;
    }
}
