using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpeechToTextUI : MonoBehaviour
{
    [SerializeField]
    private VoskSpeechToText voskSpeechToText;
    [SerializeField, Tooltip("El componente de texto en el cual mostrar el texto a leer y los resultados del reconocimiento de voz")]
    private TextMeshProUGUI textGUI;

    [Header("Colors")]
    [SerializeField, Tooltip("El color del texto que no fue leído aún")]
    private Color unreadColor = Color.black;
    [SerializeField, Tooltip("El color del texto que fue leido y evaluado como CORRECTO en el resultado PARCIAL del reconocimiento de voz")]
    private Color partialCorrect = new Color(0, 0.33f, 0);
    [SerializeField, Tooltip("El color del texto que fue leido y evaluado como CORRECTO en el resultado FINAL del reconocimiento de voz")]
    private Color finalCorrect = new Color(0, 0.66f, 0);
    [SerializeField, Tooltip("El color del texto que fue leido y evaluado como INCORRECTO en el resultado PARCIAL del reconocimiento de voz")]
    private Color partialIncorrect = new Color(0.33f, 0, 0);
    [SerializeField, Tooltip("El color del texto que fue leido y evaluado como INCORRECTO en el resultado FINAL del reconocimiento de voz")]
    private Color finalIncorrect = new Color(0.66f, 0, 0);

    [Header("Event")]
    public UnityEvent OnPhraseWellRead;


    private string _textToRead;
    private string _displayedText;

    // Belongs to another class?
    private int _finalWordCount;
    private string[] _wordsToRead;


    // Could extract as public method to subscribe/unsubscribe
    // but it is already managed by toggle recording in VoskSpeechToText
    private void OnEnable()
    {
        voskSpeechToText.OnTranscriptionPartialResult += DisplayPartialResult;
        voskSpeechToText.OnTranscriptionResult += DisplayFinalResult;
    }
    private void OnDisable()
    {
        voskSpeechToText.OnTranscriptionPartialResult -= DisplayPartialResult;
        voskSpeechToText.OnTranscriptionResult -= DisplayFinalResult;
    }

    private void Update()
    {
        textGUI.text = _displayedText;
    }


    public void StartSpeechToTextAssessment(SpellSO spellSO)
    {
        SetTextToReadFromSpell(spellSO);
        DisplayTextToRead();
        SetWordsToRead();
        ResetWordCount();
        StartCoroutine(WaitForAllWordsToBeRead());
    }

    // Should recieve a string, but event uses SpellSO Parameter. Doesn't seem necessary to make a new event only for this.
    private void SetTextToReadFromSpell(SpellSO spellSO)
    {
        _textToRead = spellSO.Spell;
    }
    private void DisplayTextToRead()
    {
        _displayedText = HtmlUtility.ToColor(_textToRead, unreadColor);
    }

    private void SetWordsToRead()
    {
        _wordsToRead = _textToRead.Split();
    }

    private void ResetWordCount()
    {
        //_partialWordCount = 0;
        _finalWordCount = 0;
    }

    private IEnumerator WaitForAllWordsToBeRead()
    {
        yield return new WaitUntil(IsRead);
        EndSpeechToTextAssessment();
    }

    private bool IsRead()
    {
        return _finalWordCount == _wordsToRead.Length;
    }

    private void EndSpeechToTextAssessment()
    {
        StopCoroutine(WaitForAllWordsToBeRead());
        OnPhraseWellRead?.Invoke(); // TODO: Toggle Recording Off
        voskSpeechToText.ToggleRecording();
        _displayedText = HtmlUtility.ToColor(_textToRead, finalCorrect);

        // TODO: Delete after testing. Run from Toggle Recording in Vosk Speech To Text. 
        //voskSpeechToText.OnTranscriptionPartialResult -= DisplayPartialResult;
        //voskSpeechToText.OnTranscriptionResult -= DisplayFinalResult;
    }



    // TODO: Refactor both Displays into One. See block comment attempt below.
    // Display is actually within the if statement.
    private void DisplayPartialResult(string recognizedSpeech)
    {
        string[] recognizedWords = SpeechToWords(recognizedSpeech);

        // Ignore empty results (why are they even sent?)
        if (string.IsNullOrEmpty(recognizedWords[0]))
            return;

        int _partialWordCount = _finalWordCount; // Added so that counter increase doesn't result in formatting as final

        for (int i = 0; i < recognizedWords.Length; i++) // Compares each recognized word to the expected word. Expected word = first word that wasn't read yet in the TextToRead
        {
            // Using Comparison instead of Equality to ignore accents.
            if (string.Compare(_wordsToRead[_partialWordCount], recognizedWords[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
            {
                // Update TextGUI to display Partial Result as Correct
                _displayedText = HighlightWords(_partialWordCount, true, true);
                _partialWordCount++;
                Debug.Log("Partial Word Count " + _partialWordCount);
            }
            else
            {
                // Update TextGUI to display Partial Result as Incorrect
                _displayedText = HighlightWords(_partialWordCount, false, true);
            }
        }
    }

    private void DisplayFinalResult(string recognizedSpeech)
    {
        string[] recognizedWords = SpeechToWords(recognizedSpeech);


        if (string.IsNullOrEmpty(recognizedWords[0]))
            return;

        for (int i = 0; i < recognizedWords.Length; i++)
        {
            // Using Comparison instead of Equality to ignore accents.
            if (string.Compare(_wordsToRead[_finalWordCount], recognizedWords[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
            {
                _displayedText = HighlightWords(_finalWordCount, true, false); // Update TextGUI to display Partial Result as Correct
                _finalWordCount++;
                Debug.Log("Final Word Count " + _finalWordCount);
            }
            else
            {
                _displayedText = HighlightWords(_finalWordCount, false, false); // Update TextGUI to display Partial Result as Correct
            }
        }
    }

    // for each recognized word in recognized speech
    // Compare each recognized word in recognized speech with the expected word
    // Split the phrase into groups of words assigning different colors:
    // Read (Final Results, Partial Results, Current), Unread
    // Current color will vary according to match result

    private static string[] SpeechToWords(string recognizedSpeech)
    {
        var recognitionResult = new RecognitionResult(recognizedSpeech);
        string recognizedPhrase = recognitionResult.Phrases[0].Text;
        string[] recognizedWords = recognizedPhrase.Split();

        if (!string.IsNullOrEmpty(recognizedPhrase))
            Debug.Log("RecognizedSpeech=" + recognizedPhrase + " / Result=" + recognitionResult.Partial);

        return recognizedWords;
    }

    // TODO: Test. Attempted to separate between final and partial previously read words
    // TODO: Refactor. Extract into methods per highlight type (4).
    private string HighlightWords(int currentWordCount, bool isCorrect, bool isPartial)
    {
        // TODO: new private color as variable. Define if using final or partial colors.
        Color correct;
        Color incorrect;
        if (isPartial)
        {
            correct = partialCorrect;
            incorrect = partialIncorrect;
        }
        else
        {
            correct = finalCorrect;
            incorrect = finalIncorrect;
        }
        
        string formattedString = "";

        // Highlight correct final results
        string correctFinalWords = "";
        for (int i = 0; i < _wordsToRead.Length; i++){
            if (i < _finalWordCount)
                correctFinalWords += _wordsToRead[i] + " ";
        }
        if (!string.IsNullOrEmpty(correctFinalWords))
            formattedString += HtmlUtility.ToColor(correctFinalWords, finalCorrect);
        //Debug.Log("correctFinalWords=" + correctFinalWords + " / FinalWordCount="+_finalWordCount);

        // Highlight correct partial results
        if (isPartial)
        {
            string correctPartialWords = "";
            for (int i = 0; i < _wordsToRead.Length; i++)
            {
                if (i >= _finalWordCount && i < currentWordCount) // FIXME: Problem with counter? First Word disappears.
                    correctPartialWords += _wordsToRead[i] + " ";
            }
            if (!string.IsNullOrEmpty(correctPartialWords))
                formattedString += HtmlUtility.ToColor(correctPartialWords, partialCorrect);
            //Debug.Log("correctPartialWords=" + correctPartialWords + " / PartialWordCount=" + _partialWordCount);
        }

        // Highlight current read word according to result
        string currentWord = _wordsToRead[currentWordCount];
        if (isCorrect)
            currentWord = HtmlUtility.ToColor(currentWord, correct);
        else
            currentWord = HtmlUtility.ToColor(currentWord, incorrect);

        if (!string.IsNullOrEmpty(currentWord))
            //formattedString += HtmlUtility.ToBold(currentWord) + " "; //FIXME: Not working. Disabled.
            formattedString += currentWord + " ";
        //Debug.Log("currentWord=" + currentWord);

        // Highlight unread word according to result
        string unreadWords = "";
        for (int i = 0; i < _wordsToRead.Length; i++){
            if (i > currentWordCount && i < _wordsToRead.Length)
                unreadWords += _wordsToRead[i] + " ";
            else if (i > currentWordCount && i == _wordsToRead.Length)
                unreadWords += _wordsToRead[i];
        }
        if (!string.IsNullOrEmpty(unreadWords))
            formattedString += HtmlUtility.ToColor(unreadWords, unreadColor);
        //Debug.Log("unreadWords=" + unreadWords);

        return formattedString;
    }


    // Allow to skip using speech during tests (both in editor or development builds. Called on button click
    public void SkipSpeechToTextAssessment()
    {
        EndSpeechToTextAssessment();
    }



    //// TODO: Could run both events to CompareSpeech
    //// using Partial bool to define HighlightWords Parameters.
    //private bool CompareSpeechWithWord(string recognizedSpeech)
    //{
    //    var recognitionResult = new RecognitionResult(recognizedSpeech);
    //    string recognizedPhrase = recognitionResult.Phrases[0].Text;
    //    string[] recognizedWords = recognizedPhrase.Split();

    //    int counter;

    //    if (recognitionResult.Partial)
    //        counter = _partialWordCount;
    //    else
    //        counter = _finalWordCount;

    //    // Compares each recognized word to the expected word.
    //    // Expected word = first word that wasn't read yet in the TextToRead
    //    for (int i = 0; i < recognizedWords.Length; i++)
    //    {
    //        // Using Comparison instead of Equality to ignore accents.
    //        if (string.Compare(_wordsToRead[counter], recognizedWords[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
    //        {
    //            _displayedText = HighlightWords(_finalWordCount, false);
    //            _partialWordCount++;

    //            return true;
    //        }
    //        // cant use else here since it would stop the loop search.
    //    }

    //    return false;
    //}

    //private void Display(string recognizedSpeech)
    //{
    //    if (CompareSpeechWithWord(recognizedSpeech))
    //    {
    //        _displayedText = HighlightWords(_finalWordCount, false);
    //        _partialWordCount++;
    //    }
    //}    
}

// TODO: replace with HighlightedWords, and use List.
// TODO: Implement in SpeechToTextUI?
public class HighlightedWord
{
    public enum Highlight
    {
        Unread,
        PartialCorrect,
        PartialIncorrect,
        FinalCorrect,
        FinalIncorrect
    }
    public Highlight highlight;

    public string FormattedWord
    {
        get
        {
            switch (highlight)
            {
                case Highlight.Unread:
                    return HtmlUtility.ToColor(unformattedWord, Unread);
                case Highlight.PartialCorrect:
                    return HtmlUtility.ToColor(unformattedWord, PartialCorrect);
                case Highlight.PartialIncorrect:
                    return HtmlUtility.ToColor(unformattedWord, PartialIncorrect);
                case Highlight.FinalCorrect:
                    return HtmlUtility.ToColor(unformattedWord, FinalCorrect);
                case Highlight.FinalIncorrect:
                    return HtmlUtility.ToColor(unformattedWord, FinalIncorrect);
                default:
                    return null;
            }
        }
    }
    public string unformattedWord;


    // TODO: Replace with ScriptableObject?
    // Can I access this class to define it if it doesn't inherit from mono?
    public Color PartialCorrect;
    public Color PartialIncorrect;
    public Color FinalCorrect;
    public Color FinalIncorrect;
    public Color Unread;
}

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