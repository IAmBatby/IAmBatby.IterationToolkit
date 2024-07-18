using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    public static RandomManager Instance => Singleton<RandomManager>.GetInstance(ref _manager);

    public static List<char> alphabet = new List<char>
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
    };

    public System.Random random;


    public T GetRandomWeightedSelection<T>(T[] values, int[] weights)
    {
        int combinedWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            combinedWeight += weights[i];
        }

        if (combinedWeight == 0)
            return (values[random.Next(0, values.Length)]);

        float randomThreshold = (float)random.NextDouble();
        float raisingRandomValue = 0f;

        for (int i = 0; i < weights.Length; i++)
            if ((float)weights[i] > 0f)
            {
                raisingRandomValue += weights[i] / combinedWeight;
                if (raisingRandomValue > randomThreshold)
                    return (values[i]);
            }

        return (values[random.Next(0, values.Length)]);
    }

    public T GetRandomSelection<T>(T[] values)
    {
        int[] weights = new int[values.Length];

        for (int i = 0; i < values.Length; i++)
            weights[i] = 1;

        return (GetRandomWeightedSelection<T>(values, weights));
    }

    public T GetRandomWeightedSelection<T>(List<(T, int)> weightedValues)
    {
        List<T> values = new List<T>();
        List<int> weights = new List<int>();

        for (int i = 0; i < weightedValues.Count; i++)
        {
            values.Add(weightedValues[i].Item1);
            weights.Add(weightedValues[i].Item2);
        }

        return (GetRandomWeightedSelection(values.ToArray(), weights.ToArray()));
    }

    public int LetterCodeToSeed(string stringCode)
    {
        string newSeedCode = string.Empty;

        foreach (char letter in stringCode)
            newSeedCode += alphabet.IndexOf(letter).ToString();

        return (int.Parse(newSeedCode));
    }
}
