using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomU
{
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int k = Random.Range(0, n);
            (list[i], list[k]) = (list[k], list[i]);
        }
    }

    public static List<T> Shuffled<T>(this List<T> list)
    {
        var newList = new List<T>(list);
        Shuffle(newList);
        return newList;
    }
}
