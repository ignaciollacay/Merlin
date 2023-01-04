using UnityEngine;

public static class HtmlUtility
{
    public static string ToColor(string text, Color color)
    {
        string openTag = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">";
        string closeTag = "</color>";

        string formattedString = openTag + text + closeTag;

        return formattedString;
    }

    public static string ToBold(string text)
    {
        string openTag = "<b>";
        string closeTag = "</b>";

        string formattedString = openTag + text + closeTag;

        return formattedString;
    }
}