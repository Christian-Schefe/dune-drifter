using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayCardInstance : MonoBehaviour
{
    private readonly ComponentCache<RectTransform> rectTransform = new();
    public RectTransform RectTransform => rectTransform.Get(this);

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public int cardIndex;

    public Vector2 targetPosition;
    public Vector2 velocity;

    public void OnPlay()
    {

    }

    public void OnDraw(int cardIndex)
    {
        this.cardIndex = cardIndex;
    }

    private void Update()
    {
        RectTransform.anchoredPosition = Vector2.SmoothDamp(RectTransform.anchoredPosition, targetPosition, ref velocity, 0.1f);
    }
}
