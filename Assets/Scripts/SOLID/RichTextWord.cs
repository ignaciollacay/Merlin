using UnityEngine;

public class RichTextWord
{
    public static Color unreadColor = Color.black;
    public static Color partialCorrect = new Color(0, 0.33f, 0);
    public static Color finalCorrect = new Color(0, 0.66f, 0);
    public static Color partialIncorrect = new Color(0.33f, 0, 0);
    public static Color finalIncorrect = new Color(0.66f, 0, 0);

    public bool isRead = false;
    public bool isPartial;
    public bool isCorrect;

    public string Word;
    public string TaggedWord { get => GetTaggedWord(); }
    public ColorTag ColorTag { get => GetColorTag(); }

    public string GetTaggedWord()
    {
        switch (ColorTag)
        {
            case ColorTag.Unread:
                return HtmlUtility.ToColor(Word, unreadColor);
            case ColorTag.PartialCorrect:
                return HtmlUtility.ToColor(Word, partialCorrect);
            case ColorTag.PartialIncorrect:
                return HtmlUtility.ToColor(Word, partialIncorrect);
            case ColorTag.FinalCorrect:
                return HtmlUtility.ToColor(Word, finalCorrect);
            case ColorTag.FinalIncorrect:
                return HtmlUtility.ToColor(Word, finalIncorrect);
            default:
                return null;
        }
    }
    public ColorTag GetColorTag()
    {
        if (isRead)
            return GetReadTag();
        else
            return ColorTag.Unread;
    }

    public ColorTag GetReadTag()
    {
        if (isCorrect)
            return GetCorrectTag();
        else
            return GetIncorrectTag();
    }
    public ColorTag GetCorrectTag()
    {
        if (isPartial)
            return ColorTag.PartialCorrect;
        else
            return ColorTag.FinalCorrect;
    }
    public ColorTag GetIncorrectTag()
    {
        if (isPartial)
            return ColorTag.PartialIncorrect;
        else
            return ColorTag.FinalIncorrect;
    }

    public void SetCorrect(bool _isCorrect)
    {
        isCorrect = _isCorrect;
    }

    public void SetPartial(bool _isPartial)
    {
        isPartial = _isPartial;
    }

    public RichTextWord(string _word)
    {
        Word = _word;
    }
}

public enum ColorTag
{
    Unread,
    PartialCorrect,
    PartialIncorrect,
    FinalCorrect,
    FinalIncorrect
}