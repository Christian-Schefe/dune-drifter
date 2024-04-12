using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathU
{
    public static float Remap(float inMin, float inMax, float outMin, float outMax, float t, bool clamp = false)
    {
        float alpha = Mathf.InverseLerp(inMin, inMax, t);
        if (clamp)
        {
            alpha = Mathf.Clamp01(alpha);
        }
        return Mathf.Lerp(outMin, outMax, alpha);
    }
}
