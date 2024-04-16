using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tweens
{
    public static Tween<Q> Any<T, Q>(Action<Q> setter, Q from, Q to, float duration, Easing easing) where T : ITweenable<Q>
    {
        return new Tween<Q>().From(from).To(to).Use(setter).Duration(duration).Easing(easing);
    }

    public static Tween<Vector3> Pos(Transform target, Vector3 from, Vector3 to, float duration, Easing easing)
    {
        return Any<Vec3Tweenable, Vector3>(v => target.position = v, from, to, duration, easing);
    }

    public static Tween<float> PosX(Transform target, float from, float to, float duration, Easing easing)
    {
        return Any<FloatTweenable, float>(x => target.position = new Vector3(x, target.position.y, target.position.z), from, to, duration, easing);
    }

    public static Tween<float> PosY(Transform target, float from, float to, float duration, Easing easing)
    {
        return Any<FloatTweenable, float>(y => target.position = new Vector3(target.position.x, y, target.position.z), from, to, duration, easing);
    }

    public static Tween<float> PosZ(Transform target, float from, float to, float duration, Easing easing)
    {
        return Any<FloatTweenable, float>(z => target.position = new Vector3(target.position.x, target.position.y, z), from, to, duration, easing);
    }

    public static Tween<Quaternion> Rot(Transform target, Quaternion from, Quaternion to, float duration, Easing easing)
    {
        return Any<QuatTweenable, Quaternion>(q => target.rotation = q, from, to, duration, easing);
    }

    public static Tween<Vector3> Scale(Transform target, Vector3 from, Vector3 to, float duration, Easing easing)
    {
        return Any<Vec3Tweenable, Vector3>(v => target.localScale = v, from, to, duration, easing);
    }

    public static Tween<float> ScaleX(Transform target, float from, float to, float duration, Easing easing)
    {
        return Any<FloatTweenable, float>(x => target.localScale = new Vector3(x, target.localScale.y, target.localScale.z), from, to, duration, easing);
    }

    public static Tween<float> ScaleY(Transform target, float from, float to, float duration, Easing easing)
    {
        return Any<FloatTweenable, float>(y => target.localScale = new Vector3(target.localScale.x, y, target.localScale.z), from, to, duration, easing);
    }

    public static Tween<float> ScaleZ(Transform target, float from, float to, float duration, Easing easing)
    {
        return Any<FloatTweenable, float>(z => target.localScale = new Vector3(target.localScale.x, target.localScale.y, z), from, to, duration, easing);
    }

    public static Tween<Vector3> TweenPosTo(this Transform target, Vector3 to, float duration, Easing easing = Easing.Linear)
    {
        return Pos(target, target.position, to, duration, easing);
    }
}
