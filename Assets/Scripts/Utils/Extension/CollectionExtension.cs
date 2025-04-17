using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class CollectionExtension
{
    public static T GetRandom<T>(this List<T> items)
    {
        return items[Random.Range(0, items.Count)];
    }

    public static T GetRandom<T>(this List<T> items, Func<T, bool> predicate)
    {
        var filterItems = items.Where(predicate).ToList();
        return filterItems[Random.Range(0, filterItems.Count)];
    }
}