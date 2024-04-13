using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridData<T>
{
    public Dictionary<Vector2Int, T> data;

    public HexGridData()
    {
        data = new();
    }

    public void SetHexagonShape(int size)
    {
        if (data is null) data = new();
        else data.Clear();

        Vector2Int center = new(0, 0);

        for (int x = -50; x <= 50; x++)
        {
            for (int y = -50; y <= 50; y++)
            {
                Vector2Int pos = new(x, y);
                if (HexGrid.ManhattenDistance(pos, center) >= size) continue;
                data.Add(pos, default);
            }
        }
    }

    public void Cleanup(System.Action<Vector2Int, T> action = null)
    {
        ForEach(action);
        data.Clear();
    }

    public void Set(System.Func<Vector2Int, T> action)
    {
        foreach (var pair in data.ToList())
        {
            data[pair.Key] = action(pair.Key);
        }
    }

    public void Map(System.Func<Vector2Int, T, T> action)
    {
        foreach (var pair in data.ToList())
        {
            data[pair.Key] = action(pair.Key, pair.Value);
        }
    }

    public void ForEach(System.Action<Vector2Int, T> action)
    {
        foreach (var pair in data)
        {
            action?.Invoke(pair.Key, pair.Value);
        }
    }
}


public class HexGrid
{
    public const float SQRT_3 = 1.73205080757f;

    public static Matrix4x4 hexToSquare = new(new(1.5f, SQRT_3 * 0.5f, 0, 0), new(0, SQRT_3, 0, 0), Vector4.zero, Vector4.zero);
    public static Matrix4x4 squareToHex = hexToSquare.inverse;

    public Matrix4x4 squareToWorld;
    public Matrix4x4 worldToSquare;

    public HexGrid(Vector3 scale, Vector3 offset, Quaternion rotation)
    {
        squareToWorld = Matrix4x4.TRS(offset, rotation, scale) * Matrix4x4.Rotate(Quaternion.Euler(90, 0, 0));
        worldToSquare = squareToWorld.inverse;
    }

    public static Vector2Int Forward => new(0, 1);

    public Vector3 HexToWorld(Vector2 vec)
    {
        return SquareToWorld(HexToSquare(vec));
    }
    public Vector2 WorldToHex(Vector3 vec)
    {
        return SquareToHex(WorldToSquare(vec));
    }

    public Vector2Int WorldToHexRound(Vector3 vec)
    {
        return SquareToHexRound(WorldToSquare(vec));
    }

    public Vector3 SquareToWorld(Vector2 vec)
    {
        return (Vector3)(squareToWorld * (Vector4)vec);
    }
    public Vector2 WorldToSquare(Vector3 vec)
    {
        return (Vector2)(worldToSquare * (Vector4)vec);
    }


    public static Vector2 HexToSquare(Vector2 vec)
    {
        return (Vector2)(hexToSquare * (Vector4)vec);
    }

    public static Vector2 SquareToHex(Vector2 vec)
    {
        return (Vector2)(squareToHex * (Vector4)vec);
    }

    public static Vector2Int SquareToHexRound(Vector2 vec)
    {
        return RoundHex(SquareToHex(vec));
    }

    public static Vector2Int RoundHex(Vector2 vec)
    {
        Vector2Int round = Vector2Int.RoundToInt(vec);
        Vector2 fract = vec - round;

        if (Mathf.Abs(fract.x) >= Mathf.Abs(fract.y))
            return round + new Vector2Int(Mathf.RoundToInt(fract.x + 0.5f * fract.y), 0);
        else
            return round + new Vector2Int(0, Mathf.RoundToInt(fract.y + 0.5f * fract.x));
    }

    public static int ManhattenDistance(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.x + a.y - b.x - b.y) + Mathf.Abs(a.y - b.y)) / 2;
    }
}