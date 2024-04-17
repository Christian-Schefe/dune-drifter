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
        Log.Debug("Draw card", cardIndex);
    }

    private void OnPlay(int cardIndex, Hand hand)
    {
        var instance = cards[cardIndex];
        cards.Remove(cardIndex);

        instance.OnPlay();
        Log.Debug("Play card", cardIndex);
    }

    private void Update()
    {
        var canvas = Globals.Get<Canvas>();
        var camera = Globals.Get<Camera>();

        var mouse = Input.mousePosition;

        var localPoints = cards.Select(c =>
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(c.Value.RectTransform, mouse, null, out var point);
            return point;
        }).ToList();

        var cardHits = cards.Where(c =>
        {
            return c.Value.RectTransform.rect.Contains(localPoints[c.Key]);
        }).Select(c =>
        {
            return (Mathf.Abs(c.Value.RectTransform.rect.center.x - localPoints[c.Key].x), c.Value.cardIndex);
        }).OrderByDescending(c => c.Item1).ToList();

        if (hoveredCard is not int prevHover || cardHits.FindIndex(e => e.cardIndex == prevHover) < 0)
        {
            hoveredCard = cardHits.Count > 0 ? cardHits.FirstOrDefault().cardIndex : null;
        }

        if (hoveredCard is int card)
        {
            cards[card].RectTransform.SetAsLastSibling();
        }
        UpdateCardTransform();
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

    public void CalculateHoveredCard()
    {
        var canvas = Globals.Get<Canvas>();
        var mouseScreen = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), mouseScreen, null, out var canvasPoint);
    }

    public void UpdateCardTransform()
    {
        var canvas = Globals.Get<Canvas>();

        var canvasSize = canvas.GetComponent<RectTransform>().rect.size;

        var restScale = 0.6f;
        var cardWidth = 250f * restScale;
        var cardHeight = 350f * restScale;

        var maxWidth = canvasSize.x * 0.5f;
        var maxGap = cardWidth * 1.0f;

        var handSize = cards.Count;

        var totalWidth = Mathf.Min((handSize - 1) * maxGap, maxWidth);

        var gap = totalWidth / (handSize - 1);

        var leftCards = (handSize - 1) * 0.5f;
        var startX = leftCards * gap * -1;

        foreach (var pair in cards)
        {
            var index = pair.Key;
            var instance = pair.Value;

            bool isHovered = hoveredCard is int hovered && hovered == index;
            var hoverHeight = isHovered ? cardHeight * 0.15f : 0;

            instance.targetPosition = new Vector2(startX + index * gap, -canvasSize.y * 0.5f + cardHeight * 0.5f + hoverHeight);
            instance.targetScale = isHovered ? 1.0f : restScale;
        }
    }
}
