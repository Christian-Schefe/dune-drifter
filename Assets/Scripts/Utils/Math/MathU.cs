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

    public static Transform SetPosX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        return transform;
    }

    public static Transform SetPosY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        return transform;
    }

    public static Transform SetPosZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
        return transform;
    }

    public static Transform SetLocalPosX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        return transform;
    }

    public static Transform SetLocalPosY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        return transform;
    }

    public static Transform SetLocalPosZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        return transform;
    }

    public static Transform SetScaleX(this Transform transform, float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        return transform;
    }

    public static Transform SetScaleY(this Transform transform, float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        return transform;
    }

    public static Transform SetScaleZ(this Transform transform, float z)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        return transform;
    }

    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
    {
        if (Time.deltaTime < Mathf.Epsilon) return rot;
        var dot = Quaternion.Dot(rot, target);
        var sign = dot > 0f ? 1f : -1f;

        target.x *= sign;
        target.y *= sign;
        target.z *= sign;
        target.w *= sign;

        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;

        var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
        deriv.x -= derivError.x;
        deriv.y -= derivError.y;
        deriv.z -= derivError.z;
        deriv.w -= derivError.w;

        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }
}
