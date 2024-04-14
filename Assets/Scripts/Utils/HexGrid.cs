using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HexGridData<T>
{
    public Dictionary<Vector2Int, T> data;
    public Dictionary<Vector2Int, List<Vector2Int>> neighbours;

    public HexGridData()
    {
        data = new();
        neighbours = new();
        foreach (var key in data.Keys) neighbours.Add(key, HexGrid.Neighbours(key).Where(Inside).ToList());
    }

    public bool Inside(Vector2Int vec) => data.ContainsKey(vec);

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

    public static Vector2 IHat = new(1.5f, SQRT_3 * 0.5f);
    public static Vector2 JHat = new(0f, SQRT_3);

    public static Matrix2x2 hexToLocal = Matrix2x2.Cols(IHat, JHat);
    public static Matrix2x2 localToHex = hexToLocal.Inverse;

    public RawTransform transform;

    public HexGrid(RawTransform transform)
    {
        this.transform = transform;
    }

    public static List<Vector2Int> Neighbours(Vector2Int vec)
    {
        return Directions.Select(e => e + vec).ToList();
    }

    public static Vector2Int[] Directions = new Vector2Int[] { Forward, UpRight, DownRight, Backwards, DownLeft, UpLeft };

    public static Vector2Int Forward => new(0, 1);
    public static Vector2Int Backwards => new(0, -1);

    public static Vector2Int UpRight => new(1, 0);
    public static Vector2Int DownLeft => new(-1, 0);

    public static Vector2Int UpLeft => new(1, -1);
    public static Vector2Int DownRight => new(-1, 1);

    public Vector3 HexToWorld(Vector2 vec)
    {
        return transform.LocalToWorld(HexToLocal(vec));
    }
    public Vector2 WorldToHex(Vector3 vec)
    {
        return LocalToHex(transform.WorldToLocal(vec));
    }

    public Vector2Int WorldToHexRound(Vector3 vec)
    {
        return LocalToHexRound(transform.WorldToLocal(vec));
    }

    public Vector3 WorldToWorldRound(Vector3 vec)
    {
        return HexToWorld(WorldToHexRound(vec));
    }

    public static Vector3 HexToLocal(Vector2 hex)
    {
        var local = hexToLocal * hex;
        return new(local.x, 0, local.y);
    }

    public static Vector2 LocalToHex(Vector3 local)
    {
        var hex = localToHex * new Vector2(local.x, local.z);
        return hex;
    }

    public static Vector2Int LocalToHexRound(Vector3 vec)
    {
        return RoundHex(LocalToHex(vec));
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

    public Vector2 WorldRayToHex(Ray ray)
    {
        var worldIntersect = MathU.PlaneRayIntersect(ray, transform.Position, transform.Up);
        return WorldToHex(worldIntersect);
    }

    public Vector2Int WorldRayToHexRound(Ray ray)
    {
        var worldIntersect = MathU.PlaneRayIntersect(ray, transform.Position, transform.Up);
        return WorldToHexRound(worldIntersect);
    }

    public Vector3 WorldRayToWorldRound(Ray ray)
    {
        var worldIntersect = MathU.PlaneRayIntersect(ray, transform.Position, transform.Up);
        return WorldToWorldRound(worldIntersect);
    }

    public static int ManhattenDistance(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.x + a.y - b.x - b.y) + Mathf.Abs(a.y - b.y)) / 2;
    }

    public static float ManhattenDistance(Vector2 a, Vector2 b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.x + a.y - b.x - b.y) + Mathf.Abs(a.y - b.y)) * 0.5f;
    }
}