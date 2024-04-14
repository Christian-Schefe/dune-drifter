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

    public static Vector4 Vec4With1(this Vector2Int vec) => new(vec.x, vec.y, 0, 1);
    public static Vector4 Vec4With1(this Vector3Int vec) => new(vec.x, vec.y, vec.z, 1);
    public static Vector4 Vec4With1(this Vector2 vec) => new(vec.x, vec.y, 0, 1);
    public static Vector4 Vec4With1(this Vector3 vec) => new(vec.x, vec.y, vec.z, 1);


    public static int ComponentSum(this Vector2Int vec)
    {
        return vec.x + vec.y;
    }

    public static int ComponentSum(this Vector3Int vec)
    {
        return vec.x + vec.y + vec.z;
    }

    public static Vector3 PlaneRayIntersect(Ray ray, Vector3 planePoint, Vector3 planeNormal)
    {
        // Plane: N * (X - P) = 0
        // Ray: X(t) = O + tD

        // Substitute: N * ((O + tD) - P) = 0
        // Solve for t: N * (O - P + tD) = 0
        // N * (O - P) + t(D * N) = 0
        // t = (N * (P - O)) / (D * N)

        float denominator = Vector3.Dot(ray.direction, planeNormal);
        float numerator = Vector3.Dot(planeNormal, planePoint - ray.origin);
        float t = numerator / denominator;

        return ray.GetPoint(t);
    }
}
