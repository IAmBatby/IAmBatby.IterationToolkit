using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static int IncreaseIndex(int index, int count)
    {
        if (index != count)
            return (index + 1);
        else
            return (0);
    }

    public static int IncreaseIndex<T>(int index, List<T> collection)
    {
        if (index != collection.Count - 1)
            return (index + 1);
        else
            return (0);
    }

    public static int DecreaseIndex(int index, int count)
    {
        if (index != 0)
            return (index - 1);
        else
            return (count);
    }

    public static int DecreaseIndex<T>(int index, List<T> collection)
    {
        if (index != 0)
            return (index - 1);
        else
            return (collection.Count - 1);
    }
}
