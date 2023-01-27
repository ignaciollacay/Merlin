using System.Collections.Generic;
using UnityEngine;

public class RichTextPhrase
{
    public string phrase;
    public string taggedPhrase { get => GetTaggedPhrase(); }
    public List<RichTextWord> words = new();


    public string GetTaggedPhrase()
    {
        string phrase = "";

        for (int i = 0; i < words.Count; i++)
        {
            if (i < words.Count)
                phrase += words[i].TaggedWord + " ";
            else
                phrase += words[i].TaggedWord;
        }

        return phrase;
    }

    public List<RichTextWord> GetRichTextWords()
    {
        string[] wordsToRead = phrase.Split();
        var _richTextWords = new List<RichTextWord>();

        for (int i = 0; i < wordsToRead.Length; i++)
        {
            var newWord = new RichTextWord(wordsToRead[i]);
            _richTextWords.Add(newWord);
        }

        return _richTextWords;
    }

    public void SetPhraseCorrect()
    {
        foreach (var word in words)
        {
            word.isRead = true;
            word.isCorrect = true;
            word.isPartial = false;
        }
    }

    public RichTextPhrase(string _phrase)
    {
        phrase = _phrase;
        words = GetRichTextWords();
    }
}
