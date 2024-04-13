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

    public static Vector2Int AbsDiff(this Vector2Int a, Vector2Int b)
    {
        return (a - b).Abs();
    }

    public static Vector3Int AbsDiff(this Vector3Int a, Vector3Int b)
    {
        return (a - b).Abs();
    }

    public static Vector2Int Abs(this Vector2Int vec)
    {
        return new(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }
    public static Vector3Int Abs(this Vector3Int vec)
    {
        return new(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }

    public static int ComponentSum(this Vector2Int vec)
    {
        return vec.x + vec.y;
    }

    public static int ComponentSum(this Vector3Int vec)
    {
        return vec.x + vec.y + vec.z;
    }
}
