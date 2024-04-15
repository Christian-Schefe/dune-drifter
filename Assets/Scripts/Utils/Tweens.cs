using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweenable<T, Q> where T : Tweenable<T, Q>
{
    public Q value;
    public Tweenable(Q value) => this.value = value;
    public virtual Q LerpUnclamped(Q from, Q to, float t) { throw new NotImplementedException(); }
    public virtual Q Lerp(Q from, Q to, float t) => LerpUnclamped(from, to, Mathf.Clamp01(t));
    public static implicit operator Tweenable<T, Q>(Q val) => new(val);
    public static implicit operator Q(Tweenable<T, Q> val) => val.value;
}
public class Vec2Wrap : Tweenable<Vec2Wrap, Vector2>
{
    public Vec2Wrap(Vector2 value) : base(value) { }
    public override Vector2 LerpUnclamped(Vector2 from, Vector2 to, float t) => Vector2.LerpUnclamped(from, to, t);
}
public class Vec3Wrap : Tweenable<Vec3Wrap, Vector3>
{
    public Vec3Wrap(Vector3 value) : base(value) { }
    public override Vector3 LerpUnclamped(Vector3 from, Vector3 to, float t) => Vector3.LerpUnclamped(from, to, t);
}
public class QuatWrap : Tweenable<QuatWrap, Quaternion>
{
    public QuatWrap(Quaternion value) : base(value) { }
    public override Quaternion LerpUnclamped(Quaternion from, Quaternion to, float t) => Quaternion.SlerpUnclamped(from, to, t);
}

public struct TweenParams
{
    public float delay;
    public float duration;
    public bool loop;
    public bool pingPong;
    public int? repeatCount;
    public float repeatWait;
    public bool reverse;
    public Ease easing;

    public static TweenParams Default => new()
    {
        delay = 0,
        duration = 1,
        loop = false,
        pingPong = false,
        repeatCount = null,
        repeatWait = 0,
        reverse = false,
        easing = Ease.Linear
    };
}

public struct TweenCycle
{
    public float initialWait;
    public float duration;
    public bool reversing;

    public readonly IEnumerator GetCoroutine(Action<float> setter, Ease easing)
    {
        float currentTime = Time.time;
        float startTime = currentTime + initialWait;
        float endTime = startTime + duration;
        float inverseDuration = 1 / duration;
        while (true)
        {
            currentTime = Time.time;
            if (currentTime < startTime)
            {
                yield return null;
                continue;
            }

            if (currentTime >= endTime)
            {
                setter(reversing ? 0 : 1);
                break;
            }

            float alpha = currentTime * inverseDuration;
            setter(easing[reversing ? 1 - alpha : alpha]);

            yield return null;
        }
    }
}

public struct TweenCallbacks
{
    public Action onStart;
    public Action onComplete;
    public Action onCancel;
    public Action onFinally;

    public static TweenCallbacks Default => new()
    {
        onStart = null,
        onComplete = null,
        onCancel = null,
        onFinally = null
    };
}

public static class Tweens
{
    public static Tween Any<T, Q>(Action<Q> setter, Tweenable<T, Q> from, Tweenable<T, Q> to, float duration, Ease easing) where T : Tweenable<T, Q>
    {
        void Setter(float t) => setter(from.LerpUnclamped(from, to, t));
        return new Tween(Setter).Duration(duration).Easing(easing);
    }

    public static Tween Pos(Transform target, Vector3 from, Vector3 to, float duration, Ease easing)
    {
        return Any<Vec3Wrap, Vector3>(v => target.position = v, from, to, duration, easing);
    }

    public static Tween Quat(Transform target, Quaternion from, Quaternion to, float duration, Ease easing)
    {
        return Any<QuatWrap, Quaternion>(q => target.rotation = q, from, to, duration, easing);
    }

    public static Tween Scale(Transform target, Vector3 from, Vector3 to, float duration, Ease easing)
    {
        return Any<Vec3Wrap, Vector3>(v => target.localScale = v, from, to, duration, easing);
    }
}

public struct Tween
{
    private TweenParams parameters;
    private TweenCallbacks callbacks;

    private readonly Action<float> setter;

    public bool Terminated { get; private set; }

    public Tween(Action<float> setter) : this(setter, TweenParams.Default, TweenCallbacks.Default) { }

    public Tween(Action<float> setter, TweenParams parameters, TweenCallbacks callbacks)
    {
        this.setter = setter;
        this.parameters = parameters;
        this.callbacks = callbacks;
        Terminated = false;
    }

    public Tween Delay(float delay)
    {
        parameters.delay = delay;
        return this;
    }

    public Tween Duration(float duration)
    {
        parameters.duration = duration;
        return this;
    }

    public Tween Loop(bool loop)
    {
        parameters.loop = loop;
        parameters.pingPong &= !loop;
        return this;
    }

    public Tween Loop(int? count)
    {
        return RepeatCount(count).Loop(count.HasValue);
    }

    public Tween PingPong(bool pingPong)
    {
        parameters.pingPong = pingPong;
        parameters.loop &= !pingPong;
        return this;
    }

    public Tween PingPong(int? count)
    {
        return RepeatCount(count).PingPong(count.HasValue);
    }

    public Tween RepeatCount(int? count)
    {
        if (count is int c && c < 1)
        {
            throw new ArgumentException("Loop count must be greater than 0");
        }
        parameters.repeatCount = count;
        return this;
    }

    public Tween RepeatWait(float wait)
    {
        parameters.repeatWait = wait;
        return this;
    }

    public Tween Reverse(bool reverse)
    {
        parameters.reverse = reverse;
        return this;
    }

    public Tween Easing(Ease easing)
    {
        parameters.easing = easing;
        return this;
    }

    public Tween OnStart(Action action, bool replace = false)
    {
        if (replace) callbacks.onStart = action;
        else callbacks.onStart += action;
        return this;
    }

    public Tween OnComplete(Action action, bool replace = false)
    {
        if (replace) callbacks.onComplete = action;
        else callbacks.onComplete += action;
        return this;
    }

    public Tween OnCancel(Action action, bool replace = false)
    {
        if (replace) callbacks.onCancel = action;
        else callbacks.onCancel += action;
        return this;
    }

    public Tween OnFinally(Action action, bool replace = false)
    {
        if (replace) callbacks.onFinally = action;
        else callbacks.onFinally += action;
        return this;
    }

    public TweenRoutine Start(MonoBehaviour owner, TweenRoutine prev = null)
    {
        if (prev != null) return prev.Replace(this);
        else return new TweenRoutine(owner.StartCoroutine(Coroutine()), this, owner);
    }

    private readonly TweenCycle GetCycle(int index)
    {
        bool flipReverse = parameters.pingPong && index % 2 == 1;
        return new TweenCycle
        {
            initialWait = index == 0 ? 0 : parameters.repeatWait,
            duration = parameters.duration,
            reversing = flipReverse ^ parameters.reverse,
        };
    }

    private readonly IEnumerator<TweenCycle> GetCycles()
    {
        if (!parameters.loop && !parameters.pingPong)
        {
            yield return new TweenCycle
            {
                initialWait = parameters.delay,
                duration = parameters.duration,
                reversing = parameters.reverse
            };
            yield break;
        }

        int index = 0;
        while (!(parameters.repeatCount is int count && index >= count))
        {
            yield return GetCycle(index);
            index++;
        }
    }

    public IEnumerator Coroutine()
    {
        var cycles = GetCycles();
        var startTime = Time.time + parameters.delay;
        while (Time.time < startTime) yield return null;
        callbacks.onStart?.Invoke();
        while (cycles.MoveNext())
        {
            yield return cycles.Current.GetCoroutine(setter, parameters.easing);
        }
        callbacks.onComplete?.Invoke();
        callbacks.onFinally?.Invoke();
        Terminated = true;
    }

    public void OnCancel()
    {
        if (!Terminated)
        {
            Terminated = true;
            callbacks.onCancel?.Invoke();
            callbacks.onFinally?.Invoke();
        }
    }
}

public class TweenRoutine
{
    public Coroutine coroutine;
    public Tween tween;
    public MonoBehaviour owner;

    public Queue<Tween> chain = new();

    public TweenRoutine(Coroutine coroutine, Tween tween, MonoBehaviour owner)
    {
        tween.OnComplete(OnComplete);

        this.coroutine = coroutine;
        this.tween = tween;
        this.owner = owner;

    }

    public void Cancel()
    {
        owner.StopCoroutine(coroutine);
        tween.OnCancel();
    }

    public TweenRoutine Replace(Tween newTween)
    {
        Cancel();
        return newTween.Start(owner);
    }

    private void OnComplete()
    {
        if (chain.Count > 0)
        {
            var newRuntime = chain.Dequeue().Start(owner, this);
            coroutine = newRuntime.coroutine;
            tween = newRuntime.tween;
            owner = newRuntime.owner;
        }
    }

    public TweenRoutine Chain(Tween nextTween)
    {
        chain.Enqueue(nextTween);
        return this;
    }
}

public sealed class Ease
{
    private readonly Func<float, float> func;

    private Ease(Func<float, float> easing)
    {
        func = easing;
    }

    public readonly static Ease Linear = new(t => t);
    public readonly static Ease QuadIn = In(2);
    public readonly static Ease QuadOut = Out(2);
    public readonly static Ease QuadInOut = InOut(2);
    public readonly static Ease CubicIn = In(3);
    public readonly static Ease CubicOut = Out(3);
    public readonly static Ease CubicInOut = InOut(3);
    public readonly static Ease QuartIn = In(4);
    public readonly static Ease QuartOut = Out(4);
    public readonly static Ease QuartInOut = InOut(4);
    public readonly static Ease QuintIn = In(5);
    public readonly static Ease QuintOut = Out(5);
    public readonly static Ease QuintInOut = InOut(5);


    public float this[float t] => func(t);

    public static Ease InOut(float power)
    {
        return new(t => t < 0.5f ? 0.5f * Mathf.Pow(2 * t, power) : 1 - 0.5f * Mathf.Pow(2 - 2 * t, power));
    }
    public static Ease In(float power)
    {
        return new(t => Mathf.Pow(t, power));
    }
    public static Ease Out(float power)
    {
        return new(t => 1 - Mathf.Pow(1 - t, power));
    }
}
