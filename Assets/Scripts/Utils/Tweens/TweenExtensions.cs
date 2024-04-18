using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TweenExtensions
{
    public static Tween<RawTransform> TweenTransform(this MonoBehaviour owner)
    {
        return new Tween<RawTransform>(owner).Use(t => t.Transfer(owner.transform, false)).From(new(owner.transform, false));
    }

    public static Tween<RawTransform> TweenLocalTransform(this MonoBehaviour owner)
    {
        return new Tween<RawTransform>(owner).Use(t => t.Transfer(owner.transform, true)).From(new(owner.transform, true));
    }

    public static Tween<Vector2> TweenAnchoredPosition(this MonoBehaviour owner)
    {
        var rectTransform = owner.GetComponent<RectTransform>();
        return new Tween<Vector2>(owner).Use(p => rectTransform.anchoredPosition = p).From(rectTransform.anchoredPosition);
    }

    public static Tween<float> TweenAnchoredPositionX(this MonoBehaviour owner)
    {
        var rectTransform = owner.GetComponent<RectTransform>();
        return new Tween<float>(owner).Use(p => rectTransform.SetAnchoredPosX(p)).From(rectTransform.anchoredPosition.x);
    }

    public static Tween<float> TweenAnchoredPositionY(this MonoBehaviour owner)
    {
        var rectTransform = owner.GetComponent<RectTransform>();
        return new Tween<float>(owner).Use(p => rectTransform.SetAnchoredPosY(p)).From(rectTransform.anchoredPosition.y);
    }

    public static Tween<Vector3> TweenPosition(this MonoBehaviour owner)
    {
        return new Tween<Vector3>(owner).Use(p => owner.transform.position = p).From(owner.transform.position);
    }

    public static Tween<float> TweenPositionX(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(x => owner.transform.SetPosX(x)).From(owner.transform.position.x);
    }

    public static Tween<float> TweenPositionY(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(y => owner.transform.SetPosY(y)).From(owner.transform.position.y);
    }

    public static Tween<float> TweenPositionZ(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(z => owner.transform.SetPosZ(z)).From(owner.transform.position.z);
    }

    public static Tween<Vector3> TweenLocalPosition(this MonoBehaviour owner)
    {
        return new Tween<Vector3>(owner).Use(p => owner.transform.localPosition = p).From(owner.transform.localPosition);
    }

    public static Tween<float> TweenLocalPositionX(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(x => owner.transform.SetLocalPosX(x)).From(owner.transform.localPosition.x);
    }

    public static Tween<float> TweenLocalPositionY(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(y => owner.transform.SetLocalPosY(y)).From(owner.transform.localPosition.y);
    }

    public static Tween<float> TweenLocalPositionZ(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(z => owner.transform.SetLocalPosZ(z)).From(owner.transform.localPosition.z);
    }

    public static Tween<Quaternion> TweenRotation(this MonoBehaviour owner)
    {
        return new Tween<Quaternion>(owner).Use(r => owner.transform.rotation = r).From(owner.transform.rotation);
    }

    public static Tween<Quaternion> TweenLocalRotation(this MonoBehaviour owner)
    {
        return new Tween<Quaternion>(owner).Use(r => owner.transform.localRotation = r).From(owner.transform.localRotation);
    }

    public static Tween<Vector3> TweenScale(this MonoBehaviour owner)
    {
        return new Tween<Vector3>(owner).Use(p => owner.transform.localScale = p).From(owner.transform.localScale);
    }

    public static Tween<float> TweenScaleX(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(x => owner.transform.SetScaleX(x)).From(owner.transform.localScale.x);
    }

    public static Tween<float> TweenScaleY(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(y => owner.transform.SetScaleY(y)).From(owner.transform.localScale.y);
    }

    public static Tween<float> TweenScaleZ(this MonoBehaviour owner)
    {
        return new Tween<float>(owner).Use(z => owner.transform.SetScaleZ(z)).From(owner.transform.localScale.z);
    }
}
