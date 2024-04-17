using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TweenParams
{
    public float delay;
    public float duration;
    public bool loop;
    public bool pingPong;
    public int? repeatCount;
    public float repeatWait;
    public bool reverse;
    public Easing easing;

    public static TweenParams Default => new()
    {
        delay = 0,
        duration = 1,
        loop = false,
        pingPong = false,
        repeatCount = null,
        repeatWait = 0,
        reverse = false,
        easing = Easing.Linear
    };
}

public struct TweenCycle
{
    public float initialWait;
    public float duration;
    public bool reversing;

    public readonly IEnumerator GetCoroutine(Action<float> setter, Easing easing)
    {
        var easingFunc = easing.Func();

        float startTime = Time.time + initialWait;
        float inverseDuration = 1 / duration;
        while (true)
        {
            var timeDelta = Time.time - startTime;

            if (timeDelta < 0)
            {
                yield return null;
                continue;
            }
            if (timeDelta >= duration)
            {
                setter?.Invoke(reversing ? 0 : 1);
                break;
            }

            float alpha = timeDelta * inverseDuration;
            setter?.Invoke(easingFunc(reversing ? 1 - alpha : alpha));

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

public class Tween<T> : Tween
{
    private Tweenable<T> from;
    private Tweenable<T> to;
    private Action<T> valSetter;

    public Tween(MonoBehaviour owner) : base(owner)
    {
        void Setter(float t) => valSetter?.Invoke(from.Lerp(to.Value, t));
        Use(Setter);
    }

    public Tween() : this(null) { }

    public Tween<T> From(T from)
    {
        this.from = (Tweenable<T>)from;
        return this;
    }

    public Tween<T> To(T to)
    {
        this.to = (Tweenable<T>)to;
        return this;
    }

    public Tween<T> By(T by)
    {
        to = (Tweenable<T>)from.Offset(by);
        return this;
    }

    public Tween<T> Use(Action<T> setter, bool replace = false)
    {
        if (replace) valSetter = setter;
        else valSetter += setter;
        return this;
    }

    public new Tween<T> Owner(MonoBehaviour owner) => (Tween<T>)base.Owner(owner);
    public new Tween<T> Delay(float delay) => (Tween<T>)base.Delay(delay);
    public new Tween<T> Duration(float duration) => (Tween<T>)base.Duration(duration);
    public new Tween<T> Loop(bool loop = true) => (Tween<T>)base.Loop(loop);
    public new Tween<T> Loop(int? count) => (Tween<T>)base.Loop(count);
    public new Tween<T> PingPong(bool pingPong = true) => (Tween<T>)base.PingPong(pingPong);
    public new Tween<T> PingPong(int? count) => (Tween<T>)base.PingPong(count);
    public new Tween<T> RepeatCount(int? count) => (Tween<T>)base.RepeatCount(count);
    public new Tween<T> RepeatWait(float wait) => (Tween<T>)base.RepeatWait(wait);
    public new Tween<T> Reverse(bool reverse = true) => (Tween<T>)base.Reverse(reverse);
    public new Tween<T> Ease(Easing easing) => (Tween<T>)base.Ease(easing);
    public new Tween<T> OnStart(Action action, bool replace = false) => (Tween<T>)base.OnStart(action, replace);
    public new Tween<T> OnComplete(Action action, bool replace = false) => (Tween<T>)base.OnComplete(action, replace);
    public new Tween<T> OnCancel(Action action, bool replace = false) => (Tween<T>)base.OnCancel(action, replace);
    public new Tween<T> OnFinally(Action action, bool replace = false) => (Tween<T>)base.OnFinally(action, replace);
}

[Serializable]
public class Tween
{
    public TweenParams parameters;
    public TweenCallbacks callbacks;
    public MonoBehaviour owner;

    protected Action<float> setter;

    public bool Terminated { get; private set; }

    public Tween() : this(null) { }

    public Tween(MonoBehaviour owner)
    {
        setter = null;
        parameters = TweenParams.Default;
        callbacks = TweenCallbacks.Default;
        Terminated = false;
        this.owner = owner;
    }

    public Tween Use(Action<float> setter, bool replace = false)
    {
        if (replace) this.setter = setter;
        else this.setter += setter;
        return this;
    }

    public Tween Owner(MonoBehaviour owner)
    {
        this.owner = owner;
        return this;
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

    public Tween Loop(bool loop = true)
    {
        parameters.loop = loop;
        parameters.pingPong &= !loop;
        return this;
    }

    public Tween Loop(int? count)
    {
        return RepeatCount(count).Loop(count.HasValue);
    }

    public Tween PingPong(bool pingPong = true)
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

    public Tween Reverse(bool reverse = true)
    {
        parameters.reverse = reverse;
        return this;
    }

    public Tween Ease(Easing easing)
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

    private bool Runnable => owner != null;
    private void AssertRunnable()
    {
        UnityEngine.Assertions.Assert.IsTrue(Runnable, "Can only run Tween if field 'Owner' is set");
    }

    public TweenRoutine RunNew()
    {
        AssertRunnable();
        TweenRoutine runner = new();
        return runner.RunQueued(this);
    }

    public TweenRoutine RunImmediate(ref TweenRoutine runner)
    {
        AssertRunnable();
        runner ??= new();
        return runner.RunImmediate(this);
    }

    public TweenRoutine RunQueued(ref TweenRoutine runner)
    {
        AssertRunnable();
        runner ??= new();
        return runner.RunQueued(this);
    }

    private TweenCycle GetCycle(int index)
    {
        bool flipReverse = parameters.pingPong && index % 2 == 1;
        return new TweenCycle
        {
            initialWait = index == 0 ? 0 : parameters.repeatWait,
            duration = parameters.duration,
            reversing = flipReverse ^ parameters.reverse,
        };
    }

    private IEnumerator<TweenCycle> GetCycles()
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
    private Coroutine coroutine;
    private Tween tween;

    private readonly Queue<Tween> chain = new();

    public bool Running { get; private set; }

    private void StartTween(Tween tween)
    {
        tween.OnComplete(OnDrawNextTween);
        coroutine = tween.owner.StartCoroutine(tween.Coroutine());
        this.tween = tween;
        Running = true;
    }

    private void OnDrawNextTween()
    {
        if (chain.Count > 0) StartTween(chain.Dequeue());
        else
        {
            coroutine = null;
            tween = null;
            Running = false;
        }
    }

    public TweenRoutine Cancel()
    {
        chain.Clear();
        return Skip();
    }

    public TweenRoutine Skip()
    {
        if (Running)
        {
            tween.owner.StopCoroutine(coroutine);
            tween.OnCancel();
        }
        OnDrawNextTween();

        return this;
    }

    public TweenRoutine RunImmediate(Tween newTween)
    {
        Cancel();
        StartTween(newTween);
        return this;
    }

    public TweenRoutine RunQueued(Tween nextTween)
    {
        if (Running) chain.Enqueue(nextTween);
        else StartTween(nextTween);
        return this;
    }
}
