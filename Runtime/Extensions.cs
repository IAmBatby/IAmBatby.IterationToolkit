using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static string ToBold(this string input)
    {
        return new string("<b>" + input + "</b>");
    }

    public static string Colorize(this string input)
    {
        string hexColor = "#" + "FFFFFF";
        return new string("<color=" + hexColor + ">" + input + "</color>");
    }

    public static string Colorize(this string input, Color color)
    {
        string hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
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

    public static string ToItalic(this string str) => "<i>" + str + "</i>";

    public static int Increase<T>(this int value, List<T> collection)
    {
        return (Utilities.IncreaseIndex(value, collection));
    }

    public static int Decrease<T>(this int value, List<T> collection)
    {
        return (Utilities.DecreaseIndex(value, collection));
    }

}