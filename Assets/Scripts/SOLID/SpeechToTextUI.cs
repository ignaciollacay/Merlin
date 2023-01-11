using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Sets the text to be read
// Recieves Speech To Text Result
// Compares the text to be read with Speech To Text result
// Updates the color of the text with different color for correct or incorrect check

public class SpeechToTextUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoskSpeechToText voskSpeechToText;
    [SerializeField] private TextMeshProUGUI textGUI;
    [Header("Colors")]
    [SerializeField] private Color unreadColor = Color.black;
    [SerializeField] private Color partialCorrect = new Color(0, 0.33f, 0);
    [SerializeField] private Color finalCorrect = new Color(0, 0.66f, 0);
    [SerializeField] private Color partialIncorrect = new Color(0.33f, 0, 0);
    [SerializeField] private Color finalIncorrect = new Color(0.66f, 0, 0);
    [Header("Event")]
    public UnityEvent OnPhraseWellRead;

    private string _textToRead;
    private string _displayedText;

    private int _finalWordCount;
    //private string[] _wordsToRead;

    private RichTextWord[] _richTextWords;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            AnalizeSpeechResult("");
        }

        textGUI.text = _displayedText;
    }

    private void OnEnable()
    {
        voskSpeechToText.OnTranscriptionPartialResult += AnalizeSpeechResult;
        voskSpeechToText.OnTranscriptionResult += AnalizeSpeechResult;
    }
    //private void OnDisable()
    //{
    //    voskSpeechToText.OnTranscriptionPartialResult -= DisplayPartialResult;
    //    voskSpeechToText.OnTranscriptionResult -= DisplayFinalResult;
    //}

    public void StartNewSpeechToTextAssessment(string textToRead)
    {
        _textToRead = textToRead;
        GetRichTextWords();
        _displayedText = GetRichTextPhrase();
        _finalWordCount = 0;
    }

    private void EndSpeechToTextAssessment()
    {
        OnPhraseWellRead?.Invoke();
        _displayedText = HtmlUtility.ToColor(_textToRead, finalCorrect);
    }

    public void AnalizeSpeechResult(string recognizedSpeech)
    {
        var recognitionResult = new RecognitionResult(recognizedSpeech);

        if (string.IsNullOrEmpty(recognitionResult.Phrases[0].Text))
            return;

        int foundWords = FindInTextToRead(recognitionResult);
        if (!recognitionResult.Partial)
        {
            _finalWordCount = foundWords;
        }

        if (AllWordsAreRead())
        {
            EndSpeechToTextAssessment();
        }
    }

    /* FIXME: Inaccurate results in long recognized phrases with repeated words
     * Long Speech To Text results allow for errors in repeated words in Text to read.
     * Can't use precise word count, since some words may be recognized with different amount of words.
     * i.e. "del" may be recognized as "de el", breaking word count in different recognized phrases.
     * Maybe should use some kind of range of words preceding and following instead of all words for more accurate results 
     */
    /* TODO: 1. Polish. Move return out of Phrase & Word For Loops
     * Polish? use a condition with counter & recognized phrase length to return or mark incorrect. RecognizedWord for loop could use a word variable, and could check it here.
     * Internal counter variable from 0
     * recognizedWordCount variable in for loop
     * Remove return from RecognizedWord loop
     * Only have return values after the Word Search.
     * Not sure if it would work
     */
    /* TODO: 2. Fix skip alternative when returning to first result after alternative
     * For more efficiency, continue on last recognized word count (avoid restarting word counter when returning to first result from alternative
     * Counter of found words, and set the recognized word loop manually?
     * Set word counter from the phrase loop? 
     * Instead of continue with returnedFromAlternative counter, use phrase counter to decide if it should set the word ienumerator of recognizedWords to foundWord Counter
     */
    // Compare recognized words in recognized phrases to find a match, and updates text color according to result.
    private int FindInTextToRead(RecognitionResult recognitionResult)
    {
        ColorTag correctTag;
        ColorTag incorrectTag;
        if (recognitionResult.Partial)
        {
            correctTag = ColorTag.PartialCorrect;
            incorrectTag = ColorTag.PartialIncorrect;
        }
        else
        {
            correctTag = ColorTag.FinalCorrect;
            incorrectTag = ColorTag.FinalIncorrect;
        }

        int wordCount = _finalWordCount; //Wanted to have an internal counter from 0, but can't update word count without final word count

        for (int phrase = 0; phrase < recognitionResult.Phrases.Length; phrase++) // for each recognized phrase (alternative)
        {
            string recognizedPhrase = recognitionResult.Phrases[phrase].Text;
            string[] recognizedWords = recognizedPhrase.Split();
            for (int word = 0; word < recognizedWords.Length; word++) // for each recognized word
            {
                string expectedWord = _richTextWords[wordCount].Word;
                string recognizedWord = recognizedWords[word];

                if (string.Compare(recognizedWord, expectedWord, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                {
                    Debug.Log("Found match. " + recognizedWord + " - " + expectedWord + " in Phrase " + phrase);

                    // innecesario updatea del displayedText en final results, pero si para partial.
                    _richTextWords[wordCount].ColorTag = correctTag;
                    _richTextWords[wordCount].TaggedWord = GetRichTextWord(_richTextWords[wordCount]);
                    _displayedText = GetRichTextPhrase();

                    wordCount++;

                    // Stop searching if all expected words have been found
                    if (wordCount >= _richTextWords.Length || // if all expected words were recognized
                        word >= (recognizedWords.Length - 1)) // if all words in read phrase were correct. Want to return before ending automatically the loop, otherwise it will reach incorrect code lines.
                    {
                        return wordCount;
                    }
                    // in first result continue search in next recognized word
                    else if (phrase == 0) 
                    {
                        continue;
                    }
                    // in alternatives continue search in next recognized phrase
                    else
                    {
                        phrase = -1; // Loop iterator will add +1
                        break;
                    }
                }
                else
                {
                    Debug.Log("No match for " + expectedWord + " in Word " + word + " of Phrase " + phrase + ": " + recognizedPhrase);
                    // in first result continue search in next recognized phrase
                    if (phrase == 0)
                    {
                        break;
                    }
                    // in alternative continue search in next recognized word.
                    else
                    {
                        continue;
                    }
                }
            }
        }
        Debug.Log("Couldn't find " + _richTextWords[wordCount].Word + " in any of the " + recognitionResult.Phrases.Length + " recognized phrases");
        _richTextWords[wordCount].ColorTag = incorrectTag;
        _richTextWords[wordCount].TaggedWord = GetRichTextWord(_richTextWords[wordCount]);
        _displayedText = GetRichTextPhrase();
        return wordCount;
    }

    // Allow to skip using speech during tests (both in editor or development builds. Called on button click
    public void SkipSpeechToTextAssessment()
    {
        EndSpeechToTextAssessment();
    }

    private void GetRichTextWords()
    {
        string[] wordsToRead = _textToRead.Split();
        _richTextWords = new RichTextWord[wordsToRead.Length];

        for (int i = 0; i < wordsToRead.Length; i++)
        {
            RichTextWord richTextWord = new RichTextWord();
            _richTextWords[i] = richTextWord;
            _richTextWords[i].Word = wordsToRead[i];
            _richTextWords[i].ColorTag = ColorTag.Unread;
            _richTextWords[i].TaggedWord = GetRichTextWord(_richTextWords[i]);
            Debug.Log(_richTextWords[i].Word);
        }
    }

    private bool AllWordsAreRead()
    {
        return _finalWordCount >= _richTextWords.Length;
    }

    public string GetRichTextWord(RichTextWord richTextWord)
    {
        switch (richTextWord.ColorTag)
        {
            case ColorTag.Unread:
                return HtmlUtility.ToColor(richTextWord.Word, unreadColor);
            case ColorTag.PartialCorrect:
                return HtmlUtility.ToColor(richTextWord.Word, partialCorrect);
            case ColorTag.PartialIncorrect:
                return HtmlUtility.ToColor(richTextWord.Word, partialIncorrect);
            case ColorTag.FinalCorrect:
                return HtmlUtility.ToColor(richTextWord.Word, finalCorrect);
            case ColorTag.FinalIncorrect:
                return HtmlUtility.ToColor(richTextWord.Word, finalIncorrect);
            default:
                return null;
        }
    }

    public string GetRichTextPhrase()
    {
        string phrase = "";

        for (int i = 0; i < _richTextWords.Length; i++)
        {
            if (i < _richTextWords.Length)
                phrase += _richTextWords[i].TaggedWord + " ";
            else
                phrase += _richTextWords[i].TaggedWord;
        }

        return phrase;
    }
}

public static class RecognitionResultCreator
{
    public static RecognitionResult result;

    public static RecognitionResult getCorrectResultAlternatives()
    {
        RecognitionResult result = new RecognitionResult();
        result.Phrases = new RecognizedPhrase[3];
        RecognizedPhrase phrase1 = new RecognizedPhrase();
        RecognizedPhrase phrase2 = new RecognizedPhrase();
        RecognizedPhrase phrase3 = new RecognizedPhrase();
        result.Phrases[0] = phrase1;
        result.Phrases[1] = phrase2;
        result.Phrases[2] = phrase3;
        result.Phrases[0].Text = "te curan de fortaleza";
        result.Phrases[1].Text = "te curan de fortalece";
        result.Phrases[2].Text = "te curen de fortaleza";

        return result;
    }

    public static RecognitionResult getIncorrectResult()
    {
        RecognitionResult result = new RecognitionResult();
        result.Phrases = new RecognizedPhrase[3];
        RecognizedPhrase phrase1 = new RecognizedPhrase();
        RecognizedPhrase phrase2 = new RecognizedPhrase();
        RecognizedPhrase phrase3 = new RecognizedPhrase();
        result.Phrases[0] = phrase1;
        result.Phrases[1] = phrase2;
        result.Phrases[2] = phrase3;
        result.Phrases[0].Text = "te curan de fortaleza";
        result.Phrases[1].Text = "te curan de fortaleza";
        result.Phrases[2].Text = "te curen de fortaleza";

        return result;
    }
}
public class RichTextPhrase
{
    public string phrase;
    public RichTextWord[] words;
}

public class RichTextWord
{
    public string Word;
    public string TaggedWord; // TODO: Make property? Should make GetRichTextWord obsolete? No tanto, le necesito definir cual es el color del tag
    public ColorTag ColorTag;
}

public class ColorCode : ScriptableObject
{
    public Color unreadColor = Color.black;
    public Color partialCorrect = new Color(0, 0.33f, 0);
    public Color finalCorrect = new Color(0, 0.66f, 0);
    public Color partialIncorrect = new Color(0.33f, 0, 0);
    public Color finalIncorrect = new Color(0.66f, 0, 0);

    public RichTextPhrase phrase;

    public string AddColorTag(RichTextWord richTextWord)
    {
        switch (richTextWord.ColorTag)
        {
            case ColorTag.Unread:
                return HtmlUtility.ToColor(richTextWord.Word, unreadColor);
            case ColorTag.PartialCorrect:
                return HtmlUtility.ToColor(richTextWord.Word, partialCorrect);
            case ColorTag.PartialIncorrect:
                return HtmlUtility.ToColor(richTextWord.Word, partialIncorrect);
            case ColorTag.FinalCorrect:
                return HtmlUtility.ToColor(richTextWord.Word, finalCorrect);
            case ColorTag.FinalIncorrect:
                return HtmlUtility.ToColor(richTextWord.Word, finalIncorrect);
            default:
                return null;
        }
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