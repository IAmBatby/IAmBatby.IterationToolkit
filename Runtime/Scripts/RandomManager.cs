using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace IterationToolkit
{
    public static class RandomManager
    {

        public static List<char> Alphbaet { get; private set; } = new List<char>
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
    };

        public static System.Random Random { get; private set; }
        public static bool HasInitialized { get; private set; }

        public static void Initialize(int newSeed)
        {
            Random = new System.Random(newSeed);
            HasInitialized = true;
        }

        public static void Initialize(string newSeed)
        {
            Random = new System.Random(LetterCodeToSeed(newSeed));
            HasInitialized = true;
        }


        public static T GetRandomWeightedSelection<T>(T[] values, int[] weights)
        {
            int combinedWeight = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                combinedWeight += weights[i];
            }

            if (combinedWeight == 0)
            {
                Debug.LogError("RandomManager Had 0 Weight, Returning Unseeded Result!");
                return (values[Random.Next(0, values.Length - 1)]);
            }

            float randomThreshold = (float)Random.NextDouble();
            float raisingRandomValue = 0f;

            for (int i = 0; i < weights.Length; i++)
            {
                float floatWeight = (float)weights[i];
                if (floatWeight > 0f)
                {
                    raisingRandomValue += floatWeight / combinedWeight;
                    if (raisingRandomValue > randomThreshold)
                        return (values[i]);
                }
            }


            Debug.LogError("RandomManager Could Not Find Seeded Result, Returning Unseeded Result! RaisingRandomValue Was: " + raisingRandomValue + ", Random Threshold Was: " + randomThreshold);
            return (values[Random.Next(0, values.Length - 1)]);
        }

        public static T GetRandomSelection<T>(List<T> values)
        {
            return (GetRandomSelection<T>(values.ToArray()));
        }

        public static T GetRandomSelection<T>(T[] values)
        {
            int[] weights = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
                weights[i] = 100;

            return (GetRandomWeightedSelection<T>(values, weights));
        }

        public static T GetRandomWeightedSelection<T>(List<(T, int)> weightedValues)
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

        public static int LetterCodeToSeed(string stringCode)
        {
            string newSeedCode = string.Empty;

            if (string.IsNullOrEmpty(stringCode))
            {
                Debug.LogWarning("String Based Seed Was Null Or Empty! Returning Random Seed.");
                return (UnityEngine.Random.Range(0, 10000));
            }

            foreach (char letter in stringCode.ToLowerInvariant().ToCharArray())
            {
                newSeedCode += Alphbaet.IndexOf(letter).ToString();
            }

            if (int.TryParse(newSeedCode, out int newSeed))
                return (newSeed);
            else
            {
                Debug.LogWarning("String Based Seed: " + newSeedCode + " Was Invalid! Returning Random Seed.");
                return (UnityEngine.Random.Range(0, 10000));
            }
        }
    }
}
