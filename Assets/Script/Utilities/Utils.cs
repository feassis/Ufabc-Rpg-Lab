using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ListExtensions
{
    /// <summary>
    /// Returns a random entry from the list.
    /// Throws an InvalidOperationException if the list is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="list">The list to select a random entry from.</param>
    /// <returns>A random element from the list.</returns>
    public static T GetRandomEntry<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("List is null or empty.");
        }

        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public static int GetRandomEntryIndex<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("List is null or empty.");
        }

        int index = UnityEngine.Random.Range(0, list.Count);
        return index;
    }

    public static System.Random r = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = r.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public static class Vector2Extensions
{
    /// <summary>
    /// Returns a random normalized Vector2 (direction).
    /// </summary>
    public static Vector2 RandomNormalized()
    {
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}