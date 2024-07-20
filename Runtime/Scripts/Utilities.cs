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
        int returnIndex = -1;

        /*
        if (index != 0)
            return (index - 1);
        else if (collection.Count != 0)
            return (collection.Count - 1);
        else
            return (0);
        */
        if (index != 0)
            returnIndex = index - 1;
        else if (collection.Count != 0)
            returnIndex = collection.Count - 1;
        else
            returnIndex = 0;

        Debug.Log("Decreasing Index. Index Was: " + index + ", Index Is: " + returnIndex + ", Collection Size Is: " + collection.Count);
        return returnIndex;
    }
}
