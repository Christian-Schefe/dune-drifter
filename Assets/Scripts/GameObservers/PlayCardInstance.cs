using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayCardInstance : GameObserver
{
    private readonly ComponentCache<RectTransform> rectTransform = new();
    public RectTransform RectTransform => rectTransform.Get(this);

    public RectTransform child;

    public TextMeshProUGUI title;
    public Image image;

    private float currentRotationOffset;

    public Vector2 targetPosition;
    public float targetRotationOffset;
    public float targetScale;

    public Vector2 posVelocity;
    public float rotVelocity;
    public Vector3 scaleVelocity;

    private PlayCard shadow;

    public int HandPos => shadow.handPosition.Value;

    private void Awake()
    {
        targetPosition = RectTransform.anchoredPosition;
        //targetRotation = RectTransform.rotation;
        targetScale = RectTransform.localScale.x;
    }

    public void SetCard(PlayCard card)
    {
        shadow = card;
    }

    protected override void OnPlayCardPlayed(PlayCard card)
    {
        if (card != shadow) return;

        this.TweenAnchoredPositionY().By(100).Ease(Easing.QuadIn).Duration(0.3f).RunNew();
        this.TweenScale().To(Vector3.zero).Ease(Easing.QuadIn).Duration(0.3f).OnFinally(() =>
        {
            Destroy(gameObject);
        }).RunNew();
    }

    private void Update()
    {
        SmoothDamp();
        child.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(posVelocity.x * 0.01f, -45, 45) + currentRotationOffset);
    }

    private void SmoothDamp()
    {
        RectTransform.anchoredPosition = Vector2.SmoothDamp(RectTransform.anchoredPosition, targetPosition, ref posVelocity, 0.1f);
        //RectTransform.rotation = MathU.SmoothDamp(RectTransform.rotation, targetRotation, ref rotVelocity, 0.1f);
        RectTransform.localScale = Vector3.SmoothDamp(RectTransform.localScale, Vector3.one * targetScale, ref scaleVelocity, 0.1f);
        currentRotationOffset = Mathf.SmoothDamp(currentRotationOffset, targetRotationOffset, ref rotVelocity, 0.1f);
    }
}
