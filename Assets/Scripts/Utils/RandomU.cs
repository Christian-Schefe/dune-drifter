using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomU
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int k = Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static List<T> Shuffled<T>(this IList<T> list)
    {
        var newList = new List<T>(list);
        Shuffle(newList);
        return newList;
    }
}
