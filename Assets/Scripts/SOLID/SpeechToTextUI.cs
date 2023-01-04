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
    private string[] _wordsToRead;

    private void Update()
    {
        textGUI.text = _displayedText;
    }

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

    public void StartNewSpeechToTextAssessment(string textToRead)
    {
        _textToRead = textToRead;
        _displayedText = HtmlUtility.ToColor(_textToRead, unreadColor);
        _wordsToRead = GetWordsToRead();
        _finalWordCount = 0;
        StartCoroutine(EndSpeechToTextAssessment());
    }

    private IEnumerator EndSpeechToTextAssessment()
    {
        yield return new WaitUntil(AllWordsAreRead);

        OnPhraseWellRead?.Invoke();
        voskSpeechToText.ToggleRecording();
        _displayedText = HtmlUtility.ToColor(_textToRead, finalCorrect);
    }

    // TODO: Refactor both Displays into One. See block comment attempt below.
    // Display is actually within the if statement.
    private void DisplayPartialResult(string recognizedSpeech)
    {
        string[] recognizedWords = GetFirstResultWords(recognizedSpeech);

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
        string[] recognizedWords = GetFirstResultWords(recognizedSpeech);


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
                // Check Next Alternative.
                string[] recognizedWordsAlternative = GetSecondResultWords(recognizedSpeech);

                Debug.Log("Expected Word=" + _wordsToRead[_finalWordCount] + " didnt match first alternative word=" + recognizedWords[i] +
                    "Testing next alternative=" + recognizedWordsAlternative[i]);

                if (string.Compare(_wordsToRead[_finalWordCount], recognizedWordsAlternative[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                {
                    Debug.Log("Matched alternative");
                    _displayedText = HighlightWords(_finalWordCount, true, false); // Update TextGUI to display Partial Result as Correct
                    _finalWordCount++;
                    Debug.Log("Final Word Count " + _finalWordCount);
                }
                else
                {
                    Debug.Log("No Match");
                    _displayedText = HighlightWords(_finalWordCount, false, false); // Update TextGUI to display Partial Result as Correct
                }
            }
        }
    }

    // for each recognized word in recognized speech
    // Compare each recognized word in recognized speech with the expected word
    // Split the phrase into groups of words assigning different colors:
    // Read (Final Results, Partial Results, Current), Unread
    // Current color will vary according to match result

    private static string[] GetFirstResultWords(string recognizedSpeech)
    {
        var recognitionResult = new RecognitionResult(recognizedSpeech);
        string recognizedPhrase = recognitionResult.Phrases[0].Text;
        string[] recognizedWords = recognizedPhrase.Split();

        if (!string.IsNullOrEmpty(recognizedPhrase))
            Debug.Log("RecognizedSpeech=" + recognizedPhrase + " / Result=" + recognitionResult.Partial);

        return recognizedWords;
    }

    private static string[] GetSecondResultWords(string recognizedSpeech)
    {
        var recognitionResult = new RecognitionResult(recognizedSpeech);
        string recognizedPhrase = recognitionResult.Phrases[1].Text;
        string[] recognizedWords = recognizedPhrase.Split();

        if (!string.IsNullOrEmpty(recognizedPhrase))
            Debug.Log("ALTERNATIVE RecognizedSpeech=" + recognizedPhrase + " / Result=" + recognitionResult.Partial);

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

    private string[] GetWordsToRead()
    {
        return _textToRead.Split();
    }

    private bool AllWordsAreRead()
    {
        return _finalWordCount == _wordsToRead.Length;
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
