using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Easing
{
    Linear,
    QuadIn,
    QuadOut,
    QuadInOut,
    CubicIn,
    CubicOut,
    CubicInOut,
    QuartIn,
    QuartOut,
    QuartInOut,
    QuintIn,
    QuintOut,
    QuintInOut
}

public static class Ease
{
    public static Func<float, float> Get(Easing easing) => easingMapping[easing];

    private static readonly Dictionary<Easing, Func<float, float>> easingMapping = new() {
        {Easing.Linear, t => t},
        {Easing.QuadIn, In(2)},
        {Easing.QuadOut, Out(2)},
        {Easing.QuadInOut, InOut(2)},
        {Easing.CubicIn, In(3)},
        {Easing.CubicOut, Out(3)},
        {Easing.CubicInOut, InOut(3)},
        {Easing.QuartIn, In(4)},
        {Easing.QuartOut, Out(4)},
        {Easing.QuartInOut, InOut(4)},
        {Easing.QuintIn, In(5)},
        {Easing.QuintOut, Out(5)},
        {Easing.QuintInOut, InOut(5)}
    };

    public static Func<float, float> InOut(float power)
    {
        return t => t < 0.5f ? 0.5f * Mathf.Pow(2 * t, power) : 1 - 0.5f * Mathf.Pow(2 - 2 * t, power);
    }
    public static Func<float, float> In(float power)
    {
        return t => Mathf.Pow(t, power);
    }
    public static Func<float, float> Out(float power)
    {
        return t => 1 - Mathf.Pow(1 - t, power);
    }
}
