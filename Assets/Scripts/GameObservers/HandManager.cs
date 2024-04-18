using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : GameObserver
{
    public PlayCardMapping mapping;

    private List<PlayCardInstance> cards = new();

    private int? hoveredCard;

    private bool isOpen;
    private bool draggingCard;
    private Vector2Int? targetedPosition;

    public const float cardScale = 0.6f;
    public const float cardWidth = 250f * cardScale;
    public const float cardHeight = 350f * cardScale;

    protected override void OnPlayCardDrawn(PlayCard card)
    {
        var entry = mapping.Get(card.type);
        var instance = entry.CreateInstance(transform);
        cards.Add(instance);
        instance.SetCard(card);
        var canvasSize = Globals.Get<Canvas>().GetComponent<RectTransform>().rect.size;
        instance.RectTransform.anchoredPosition = new(canvasSize.x * 0.5f, canvasSize.y * -0.5f);
    }

    protected override void OnPlayCardPlayed(PlayCard card)
    {
        var i = cards.FindIndex(c => c.HandPos == card.handPosition);
        cards.RemoveAt(i);
    }

    private void Update()
    {
        var canvas = Globals.Get<Canvas>();
        var camera = Globals.Get<Camera>();
        var canvasTransform = canvas.GetComponent<RectTransform>();

        var mouse = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, mouse, null, out var canvasPoint);

        isOpen = canvasPoint.y + canvasTransform.rect.height * 0.5f < cardHeight;

        var cardHits = new List<(float, int)>();
        for (int i = 0; i < cards.Count; i++)
        {
            var curCard = cards[i];
            RectTransformUtility.ScreenPointToLocalPointInRectangle(curCard.RectTransform, mouse, null, out var point);
            if (!curCard.RectTransform.rect.Contains(point)) continue;
            cardHits.Add((Mathf.Abs(curCard.RectTransform.rect.center.x - point.x), i));
        }
        var hits = cardHits.OrderByDescending(c => c.Item1).ToList();


        if (!draggingCard && (hoveredCard is not int prevHover || cardHits.FindIndex(e => e.Item2 == prevHover) < 0))
        {
            hoveredCard = cardHits.Count > 0 ? hits.FirstOrDefault().Item2 : null;
        }

        if (hoveredCard != null || draggingCard) isOpen = true;

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].RectTransform.SetSiblingIndex(i);
        }

        if (hoveredCard is int card)
        {
            cards[card].RectTransform.SetAsLastSibling();
        }
        HandleCardDrag();
        UpdateCardTransform();
    }

    public void HandleCardDrag()
    {
        var arena = Globals.Get<ArenaManager>();

        if (Input.GetMouseButtonDown(0) && Match.CardsToPlay >= 1)
        {
            if (hoveredCard is int && CurrentPlayer) draggingCard = true;
        }
        if (draggingCard && Input.GetMouseButtonUp(0))
        {
            arena.Deselect();
            draggingCard = false;
            if (targetedPosition is Vector2Int p && hoveredCard is int c)
            {
                Match.PlayCard(cards[c].HandPos, p);
            }
        }

        if (draggingCard)
        {
            arena.Deselect();
            var mouseGrid = Grid.RayIntersectRound(InputU.MouseRay);
            if (Arena.grid.Inside(mouseGrid))
            {
                arena.Select(mouseGrid);
                targetedPosition = mouseGrid;
            }
            else targetedPosition = null;
        }
    }

    public void UpdateCardTransform()
    {
        if (cards.Count == 0) return;
        var canvas = Globals.Get<Canvas>();

        var canvasSize = canvas.GetComponent<RectTransform>().rect.size;

        var maxWidth = canvasSize.x * 0.5f;
        var maxGap = cardWidth * 0.8f;

        var handSize = cards.Count;

        var totalWidth = Mathf.Min((handSize - 1) * maxGap, maxWidth);

        var gap = handSize > 1 ? totalWidth / (handSize - 1) : 0;

        var leftCards = (handSize - 1) * 0.5f;
        var startX = leftCards * gap * -1;

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];

            bool isHovered = hoveredCard is int hovered && hovered == i;
            var hoverHeight = isHovered ? cardHeight * 0.15f : 0;

            if (!isOpen) hoverHeight = -cardHeight * 0.4f;

            card.targetPosition = new Vector2(startX + i * gap, -canvasSize.y * 0.5f + cardHeight * 0.5f + hoverHeight);
            card.targetPosition.y -= Mathf.Abs(card.targetPosition.x * card.targetPosition.x * 0.0005f);

            card.targetScale = isHovered ? cardScale * 1.3f : cardScale;

            var angle = MathU.Remap(canvasSize.x * -0.5f, canvasSize.x * 0.5f, 45, -45, card.targetPosition.x, true);
            card.targetRotationOffset = isHovered ? 0 : angle;
        }
    }
}
