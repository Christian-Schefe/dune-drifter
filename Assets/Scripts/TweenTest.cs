using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    public TweenRoutine tween;

    private void Start()
    {
        this.TweenPosition().Duration(2f).By(Vector3.left).Ease(Easing.QuadInOut).PingPong(4).RunQueued(ref tween);
        this.TweenPosition().Duration(0.5f).By(Vector3.up).Ease(Easing.QuadInOut).Loop(8).RunQueued(ref tween);

        new Tween(this).Delay(5).OnComplete(() => Log.Debug("Hi")).RunNew();
    }
}
