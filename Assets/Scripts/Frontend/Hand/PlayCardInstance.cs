using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayCardInstance : MonoBehaviour
{
    private readonly ComponentCache<RectTransform> rectTransform = new();
    public RectTransform RectTransform => rectTransform.Get(this);

    public RectTransform child;

    public TextMeshProUGUI title;
    public Image image;

    public int cardIndex;

    public Vector2 targetPosition;
    //public Quaternion targetRotation;
    public float targetScale;

    public Vector2 posVelocity;
    //public Quaternion rotVelocity;
    public Vector3 scaleVelocity;

    private void Awake()
    {
        targetPosition = RectTransform.anchoredPosition;
        //targetRotation = RectTransform.rotation;
        targetScale = RectTransform.localScale.x;
    }

    public void OnPlay()
    {

    }

    public void OnDraw(int cardIndex)
    {
        this.cardIndex = cardIndex;
    }

    private void Update()
    {
        SmoothDamp();
        child.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(posVelocity.x * 0.01f, -45, 45));
    }

    private void SmoothDamp()
    {
        RectTransform.anchoredPosition = Vector2.SmoothDamp(RectTransform.anchoredPosition, targetPosition, ref posVelocity, 0.1f);
        //RectTransform.rotation = MathU.SmoothDamp(RectTransform.rotation, targetRotation, ref rotVelocity, 0.1f);
        RectTransform.localScale = Vector3.SmoothDamp(RectTransform.localScale, Vector3.one * targetScale, ref scaleVelocity, 0.1f);

    }
}
