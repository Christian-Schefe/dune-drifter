using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween
{
    private readonly Easing easing;
    private readonly float duration;
    private readonly Action<float> setter;

    private Action onFinally;
    private Action onComplete;
    private Action onCancel;

    public float alpha;

    public static Tween Vec3(Action<Vector3> setter, Vector3 from, Vector3 to, float duration, Easing easing)
    {
        void Setter(float t) => setter(Vector3.LerpUnclamped(from, to, t));
        return new Tween(Setter, duration, easing);
    }

    public static Tween Pos(Transform target, Vector3 from, Vector3 to, float duration, Easing easing)
    {
        void Setter(float t) => target.position = Vector3.LerpUnclamped(from, to, t);
        return new Tween(Setter, duration, easing);
    }

    public Tween(Action<float> setter, float duration, Easing easing)
    {
        this.easing = easing;
        this.setter = setter;
        this.duration = duration;
        alpha = 0f;
    }

    public Tween OnFinally(Action onFinally)
    {
        this.onFinally = onFinally;
        return this;
    }

    public Tween OnComplete(Action onComplete)
    {
        this.onComplete = onComplete;
        return this;
    }

    public Tween OnCancel(Action onCancel)
    {
        this.onCancel = onCancel;
        return this;
    }

    public TweenRoutine Start(MonoBehaviour owner, TweenRoutine prev = null)
    {
        if (prev != null) return prev.Replace(this);
        else return new TweenRoutine(owner.StartCoroutine(Coroutine()), this, owner);
    }

    public Tween Chain(Tween second, float durationGap = 0f)
    {
        float newDuration = duration + second.duration + durationGap;
        float t1Range = duration / newDuration;
        float t2Range = second.duration / newDuration;
        float t2Offset = newDuration - second.duration;

        void Setter(float t)
        {
            float t1 = t / t1Range;
            if (t1 <= 1) setter(easing[t1]);
            else
            {
                float t2 = (t - t2Offset) / t2Range;
                second.setter(second.easing[t2]);
            }
        }
        return new(Setter, newDuration, Easing.Linear);
    }

    public Tween Reversed()
    {
        return new(t => setter(1f - t), duration, easing);
    }

    public IEnumerator Coroutine()
    {
        float startTime = Time.time;
        float inverseDuration = 1f / duration;

        while (true)
        {
            float timeDiff = Time.time - startTime;
            alpha = timeDiff * inverseDuration;
            if (alpha >= 1f)
            {
                alpha = 1f;
                setter(alpha);
                onComplete?.Invoke();
                onFinally?.Invoke();
                break;
            }

            setter(easing[alpha]);
            yield return null;
        }
    }

    public void Cancel()
    {
        onCancel?.Invoke();
        onFinally?.Invoke();
    }
}

public class TweenRoutine
{
    public Coroutine coroutine;
    public Tween tween;
    public MonoBehaviour owner;

    public TweenRoutine(Coroutine coroutine, Tween tween, MonoBehaviour owner)
    {
        this.coroutine = coroutine;
        this.tween = tween;
        this.owner = owner;
    }

    public bool IsFinished => tween.alpha >= 1f;

    public void Cancel()
    {
        owner.StopCoroutine(coroutine);
        tween.Cancel();
    }

    public TweenRoutine Replace(Tween newTween)
    {
        Cancel();
        return newTween.Start(owner);
    }
}

public sealed class Easing
{
    private readonly Func<float, float> func;

    private Easing(Func<float, float> easing)
    {
        func = easing;
    }

    public readonly static Easing Linear = new(t => t);
    public readonly static Easing QuadIn = new(t => t * t);
    public readonly static Easing QuadOut = new(t => 1f - (1f - t) * (1f - t));
    public readonly static Easing Quad = new(t => t < 0.5f ? 2 * t * t : 1f - 2 * (1 - t) * (1 - t));
    public readonly static Easing CubicIn = new(t => t * t * t);
    public readonly static Easing CubicOut = new(t => 1f - (1 - t) * (1 - t) * (1 - t));
    public readonly static Easing Cubic = new(t => t < 0.5f ? 4 * t * t * t : 1f - 4 * (1 - t) * (1 - t) * (1 - t));

    public float this[float t] => func(t);
}