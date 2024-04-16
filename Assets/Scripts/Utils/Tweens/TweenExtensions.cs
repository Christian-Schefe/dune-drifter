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

    public static Tween<Vector3> TweenPosition(this MonoBehaviour owner)
    {
        return new Tween<Vector3>(owner).Use(p => owner.transform.position = p).From(owner.transform.position);
    }

    public static Tween<Vector3> TweenLocalPosition(this MonoBehaviour owner)
    {
        return new Tween<Vector3>(owner).Use(p => owner.transform.localPosition = p).From(owner.transform.localPosition);
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
}