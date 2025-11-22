using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit
{
    public enum RichTextType { UGUI, TMP }
    public static class Extensions
    {
        public static Color SetAlpha(this Color color, float newAlpha, bool scaledValues = false)
        {
            Color scaledColor;
            Color nonScaledColor;
            if (color.r > 1 || color.g > 1 || color.b > 1)
            {
                scaledColor = new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
                nonScaledColor = color;
            }
            else
            {
                scaledColor = color;
                nonScaledColor = new Color(color.r * 255, color.g * 255, color.b * 255, color.a * 255);
            }

            if (scaledValues == true)
                return (new Color(scaledColor.r, scaledColor.g, scaledColor.b, newAlpha));
            else
                return (new Color(nonScaledColor.r, nonScaledColor.g, nonScaledColor.b, newAlpha));
        }

        public static string ToBold(this string input) => "<b>" + input + "</b>";

        public static string ToItalic(this string str) => "<i>" + str + "</i>";

        //public static string Colorize(this string input, Color color) => "<color=" + ColorUtility.ToHtmlStringRGB(color) + ">" + input + "</color>";

        public static string Colorize(this string input, Color color, RichTextType textType = RichTextType.UGUI)
        {
            string startingTag = textType == RichTextType.UGUI ? "<color=#" : "<color=#";
            return (startingTag + ColorUtility.ToHtmlStringRGBA(color) + ">" + input + "</color>");
        }

        public static string Colorize(this string input)
        {
            string hexColor = "#" + "FFFFFF";
            return new string("<color=" + hexColor + ">" + input + "</color>");
        }

        public static string FirstToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            char[] chars = input.ToCharArray();
            if (char.IsLetter(chars[0]))
                chars[0] = char.ToUpper(chars[0]);
            return (new string(chars));
        }

        public static bool IsValidIndex<T>(this List<T> collection, int potentialIndex) => potentialIndex > -1 && potentialIndex < collection.Count;

        public static bool IsValidIndex(this Vector3Int rangeMax, Vector3Int index) => IsValidIndex(rangeMax, index, Vector3Int.zero);
        public static bool IsValidIndex(this Vector3Int rangeMax, Vector3Int index, Vector3Int rangeMin)
        {
            if (index.x < rangeMin.x || index.x > rangeMax.x) return (false);
            if (index.y < rangeMin.y || index.y > rangeMax.y) return (false);
            if (index.z < rangeMin.z || index.z > rangeMax.z) return (false);
            Debug.Log("Valid: " + index + ", Min: " + rangeMin + ", Max: " + rangeMax);
            return (true);
        }

        public static int Increase<T>(this int value, List<T> collection)
        {
            return (Utilities.IncreaseIndex(value, collection));
        }

        public static bool IsOdd(this int value)
        {
            if (value % 2 == 0)
                return (true);
            else
                return (false);
        }

        public static int Decrease<T>(this int value, List<T> collection)
        {
            return (Utilities.DecreaseIndex(value, collection));
        }

        public static void TryToggleInclusion<T>(this List<T> collection, T value)
        {
            if (collection == null || value == null) return;
            if (collection.Contains(value))
                collection.Remove(value);
            else
                collection.Add(value);
        }

        public static GUIStyle Colorize(this GUIStyle input, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            input.normal.background = texture;
            return input;
        }

        public static void Play<T>(this T source, MaterialPreset preset) where T : class, IReactable
        {
            source.ReactionController.Play(preset);
        }

        public static bool IsHighlighted<T>(this T source) where T : class, IHighlightable
        {
            return (Highlighting.Highlighted != null && Highlighting.Highlighted == source);
        }

        public static void DisplayContent<T>(this T source, Vector3 position, GUIStyle style = null) where T : MonoBehaviour, IContentDisplaySource
        {
            ContentDisplay.DrawContentDisplayGroup(position, source.ContentDisplayGroup, style);
        }

        public static void DisplayContent<T>(this T source, GUIStyle style = null) where T : MonoBehaviour, IContentDisplaySource
        {
            ContentDisplay.DrawContentDisplayGroup(source.transform.position, source.ContentDisplayGroup, style);
        }

        public static Rect AddPadding(this Rect rect, Rect padding)
        {
            return (new Rect(rect.x - padding.x, rect.y - padding.y, rect.width + padding.width, rect.height + padding.height));
        }

        public static void AddOrAddAdd<K, V>(this Dictionary<K, List<V>> dict, K key, V value)
        {
            if (key == null || value == null) return;
            if (dict.TryGetValue(key, out List<V> list) == false)
                dict.Add(key, new List<V> { value });
            else if (!list.Contains(value))
                list.Add(value);
        }


        public static Vector3Int Convert(this Direction direction, int distance)
        {
            return (direction switch
            {
                Direction.Forward => new Vector3Int(0, 0, distance),
                Direction.Backward => new Vector3Int(0, 0, -distance),
                Direction.Left => new Vector3Int(-distance, 0, 0),
                Direction.Right => new Vector3Int(distance, 0, 0),
                Direction.Up => new Vector3Int(0, distance, 0),
                Direction.Down => new Vector3Int(0, -distance, 0),
                _ => new Vector3Int(0, 0, 0)
            });
        }

        public static Vector3 IndexToPosition<T>(this IGrid<T> grid, int x, int y, int z, Space spaceType = Space.World) where T : IGridInfo, new()
        {
            return (grid.IndexToPosition(new Vector3Int(x,y,z), spaceType));
        }
        public static Vector3 IndexToPosition<T>(this IGrid<T> grid, Vector3 index, Space spaceType = Space.World) where T : IGridInfo, new()
        {
            return (grid.IndexToPosition(index, spaceType));
        }

    }
}