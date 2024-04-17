using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Matrix2x2
{
    public float m00, m01, m10, m11;

    public Matrix2x2(float m00, float m01, float m10, float m11)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m10 = m10;
        this.m11 = m11;
    }

    public static Matrix2x2 Rows(Vector2 v0, Vector2 v1) => new(v0.x, v0.y, v1.x, v1.y);
    public static Matrix2x2 Cols(Vector2 v0, Vector2 v1) => new(v0.x, v1.x, v0.y, v1.y);

    public static Matrix2x2 Identity => new(1f, 0f, 0f, 1f);

    public readonly float Determinant => m00 * m11 - m01 * m10;

    public readonly Matrix2x2 Inverse
    {
        get
        {
            float det = Determinant;
            if (Mathf.Approximately(det, 0))
            {
                Log.Err("Matrix is singular and cannot be inverted.");
                return new Matrix2x2(float.NaN, float.NaN, float.NaN, float.NaN);
            }
            float invDet = 1f / det;
            return new Matrix2x2(
                m11 * invDet, -m01 * invDet,
                -m10 * invDet, m00 * invDet
            );
        }
    }

    public static Matrix2x2 operator *(Matrix2x2 a, Matrix2x2 b)
    {
        return new Matrix2x2(
            a.m00 * b.m00 + a.m01 * b.m10,
            a.m00 * b.m01 + a.m01 * b.m11,
            a.m10 * b.m00 + a.m11 * b.m10,
            a.m10 * b.m01 + a.m11 * b.m11
        );
    }

    public static Vector2 operator *(Matrix2x2 matrix, Vector2 vector)
    {
        return new Vector2(
            matrix.m00 * vector.x + matrix.m01 * vector.y,
            matrix.m10 * vector.x + matrix.m11 * vector.y
        );
    }
}
