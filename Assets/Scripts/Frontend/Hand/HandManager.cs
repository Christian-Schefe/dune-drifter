using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public PlayCardMapping mapping;

    private Dictionary<int, PlayCardInstance> cards = new();

    private int? hoveredCard;

    private void OnDraw(int cardIndex, Hand hand)
    {
        var entry = mapping.Get(hand.drawnCards[cardIndex].Type);
        var instance = entry.CreateInstance(transform);
        cards.Add(cardIndex, instance);
        instance.OnDraw(cardIndex);
        UpdateCardPositions();
        Log.Debug("Draw card {0}", cardIndex);
    }

    private void OnPlay(int cardIndex, Hand hand)
    {
        var instance = cards[cardIndex];
        cards.Remove(cardIndex);

        instance.OnPlay();
        UpdateCardPositions();
        Log.Debug("Play card {0}", cardIndex);
    }

    private void UpdateCardPositions()
    {
        foreach (var pair in cards)
        {
            var index = pair.Key;
            var instance = pair.Value;

            instance.targetPosition = GetCardPosition(index);
        }
    }

    private void Update()
    {
        var canvas = Globals.Get<Canvas>();
        var camera = Globals.Get<Camera>();

        var mouse = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), mouse, camera, out var point);

        var cardHits = cards.Where(c => c.Value.RectTransform.rect.Contains(point)).Select(c =>
        {
            return (Mathf.Abs(c.Value.RectTransform.rect.center.x - point.x), c.Value.cardIndex);
        }).OrderByDescending(c => c.Item1).ToList();

        hoveredCard = cardHits.Count > 0 ? cardHits.FirstOrDefault().cardIndex : null;

        if (hoveredCard is int card)
        {
            cards[card].transform.SetAsFirstSibling();
        }
    }

    private void OnEnable()
    {
        Events.Get<HandSignal.Draw>().AddListener(OnDraw);
        Events.Get<HandSignal.Play>().AddListener(OnPlay);
    }

    private void OnDisable()
    {
        Events.Get<HandSignal.Draw>().RemoveListener(OnDraw);
        Events.Get<HandSignal.Play>().RemoveListener(OnPlay);
    }

    public Vector2 GetCardPosition(int index)
    {
        var canvas = Globals.Get<Canvas>();

        var canvasSize = canvas.GetComponent<RectTransform>().rect.size;

        var cardWidth = 200f;
        var cardHeight = 300f;

        var maxWidth = canvasSize.x * 0.5f;
        var maxGap = cardWidth * 2.0f;

        var handSize = cards.Count;

        var totalWidth = Mathf.Min((handSize - 1) * maxGap, maxWidth);

        var gap = totalWidth / (handSize - 1);

        var leftCards = (handSize - 1) * 0.5f;
        var startX = leftCards * gap * -1;

        var hoverHeight = cardHeight * 0.5f;

        return new Vector2(startX + index * gap, -canvasSize.y * 0.5f + cardHeight * 0.5f + hoverHeight);
    }
}
