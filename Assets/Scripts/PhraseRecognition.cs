using Microsoft.CognitiveServices.Speech;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Complement to Speech Recognition
/// Compares in real time each word of the Recognized Speech (via recognizer Event)
/// with each word of a given phrase 
/// changing the text in a karaoke-like visual feedback on screen
/// </summary>
public class PhraseRecognition : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Unity UI Text component used to post text results on screen.")]
    public Text readText;
    [Tooltip("The text that the user has to read")]
    public string readPhrase;
    private string[] readStrings;

    public string[] words;     //Segmentation in words of the phrase to be read
                               //Created automatically on awake
                               //Accessed by SpeechRecognition.CreatePhraseList()

    [Header("Font Format Settings")]
    [Tooltip("Color for incorrect read words, in string format (using HTML)")]
    [SerializeField] private string correctColor = "cyan";
    [Tooltip("Color for correct read words, in string format (using HTML)")]
    [SerializeField] private string incorrectColor = "red";
    [Tooltip("Color for unread words, in string format (using HTML)")]
    [SerializeField] private string unreadColor = "white";

    [Tooltip("Font color for current word")]
    string color;     //string for dynamic color,
                      //to hold a variable color (correct or incorrect)
                      //according to SpeechRecognition & AssessmentResult
    [Tooltip("Font size for current word")]
    [SerializeField] private int fontSize = 42;

    [Tooltip("Index # of the current word within the phrase to be recognized")]
    public int wordCount = 0;

    string displayString;    //string to hold text since it needs to run on update
                             //to change format on screen.

    //Events
    public delegate void PhraseRecognized();
    public event PhraseRecognized OnPhraseRecognized;

    void Awake()
    {
        CreateStringPhrases();
        SetText();
    }

    private void Start()
    {
        StartCoroutine(FinishedPhrase());
        //CultureInfo.CurrentCulture = new CultureInfo("es-AR");
        //Debug.Log("The current culture is {0}.\n" +
        //                System.Globalization.CultureInfo.CurrentCulture.Name);

        PhraseManager.Instance.phraseRecs.Add(this);
    }
    private void Update()
    {
        readText.text = displayString;
    }
    private void OnDisable()
    {
        StopCoroutine(FinishedPhrase());
    }

    IEnumerator FinishedPhrase()
    {
        yield return new WaitUntil(() => wordCount == words.Length);
        Debug.Log("All keywords are recognized for " + this.name, this.gameObject);
        StopAssessment();
        OnPhraseRecognized.Invoke();
    }

    /// <summary>
    /// Check if the recognized words match the phrase to be read.
    /// Run from SpeechRecognition on Update
    /// </summary>
    /// <param name="recognizedPhrase"></param>
    public void PronunciationAssessment(object sender, SpeechRecognitionEventArgs e)
    {
        string recognizedPhrase = "";

        if (e.Result.Reason == ResultReason.RecognizingSpeech)
        {
            recognizedPhrase = e.Result.Text;
        }

        string[] recognizedWords = recognizedPhrase.Split();

        for (int i = 0; i < recognizedWords.Length; i++)
        {
            //if (recognizedWords[i].Equals(words[wordCount], StringComparison.CurrentCultureIgnoreCase))
            //String Equality does not ignore accent
            //Using String Comparison instead
            if (string.Compare(words[wordCount], recognizedWords[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
            {
                CreateStringPhrase(wordCount, correctColor);
                displayString = readStrings[wordCount];
                wordCount++;
            }
            else
            {
                CreateStringPhrase(wordCount, incorrectColor);
                displayString = readStrings[wordCount];
            }
        }
    }

    /// <summary>
    /// Set ending display text. Run from SpeechRecognition once assessment finished and result was correct.
    /// </summary>
    public void StopAssessment()
    {
        displayString = readPhrase;
        readText.color = Color.cyan;
        readText.text = displayString;
        StopCoroutine(FinishedPhrase());
    }

    /// <summary>
    /// Automatical karaoke-like effect for a given phrase
    /// Creates different string highlighting each word in the phrase to be read
    /// Allowing to switch between the word that is currently read in real time
    /// using Speech & Phrase Recognition
    /// Each string is stored in phrases array.
    /// </summary>
    void CreateStringPhrases()
    {
        string col1 = "<color=" + unreadColor + ">";
        string col2 = "<color=" + correctColor + ">";
        string dynamicColor = "<color=" + color + ">";
        string colorOut = "</color>";
        string bold = "<b>";
        string size = "<size=" + fontSize + ">";
        string closeFormat = "</size></b></color>";

        words = readPhrase.Split();

        readStrings = new string[words.Length];
        //Debug.Log("Amount of words in phrase= " + readStrings.Length);

        //Repeats process for each word in the phrase
        for (int phrase = 0; phrase < readStrings.Length; phrase++)
        {
            //create a string for the phrase
            string phraseString = "";

            //Add words that were read in cyan color format
            //open cyan color format
            phraseString += col2;
            //Add read words;
            for (int word = 0; word < words.Length; word++)
            {
                if (word < phrase)
                {
                    phraseString += words[word] + " ";
                }
            }
            //close cyan color format
            phraseString += colorOut;

            //add current word in highlighted format
            phraseString += dynamicColor + bold + size + words[phrase] + closeFormat + " ";

            //Add words that were not read yet in white color format
            //open white color format
            phraseString += col1;
            //add unread words
            for (int word = 0; word < words.Length; word++)
            {
                if (word > phrase)
                {
                    phraseString += words[word] + " ";
                }
            }
            //close white color format
            phraseString += colorOut;

            //Debug.Log("Phrase" + phrase + ". Generated String=" + phraseString);
            // Set phrase to the generated string
            readStrings[phrase] = phraseString;
        }
    }
    /// <summary>
    /// Creates String for phrase to be displayed in a dynamic color
    /// according to the result of Speech & Phrase Recognition.
    /// </summary>
    /// <param name="phrase"></param>
    /// <param name="color"></param>
    void CreateStringPhrase(int phrase, string color)
    {
        string col1 = "<color=" + unreadColor + ">";
        string col2 = "<color=" + correctColor + ">";
        string dynamicColor = "<color=" + color + ">";
        string colorOut = "</color>";
        string bold = "<b>";
        string size = "<size=" + fontSize + ">";
        string closeFormat = "</size></b></color>";

        string phraseString = "";
        // Add read words;
        phraseString += col2;
        for (int word = 0; word < words.Length; word++)
        {
            if (word < phrase)
            {
                phraseString += words[word] + " ";
            }
        }
        phraseString += colorOut;
        // add current word in highlighted format
        phraseString += dynamicColor + bold + size + words[phrase] + closeFormat + " ";
        // add unread words
        phraseString += col1;
        for (int word = 0; word < words.Length; word++)
        {
            if (word > phrase)
            {
                phraseString += words[word] + " ";
            }
        }
        phraseString += colorOut;

        //set phrase to the generated string
        readStrings[phrase] = phraseString;
    }


    public void SetText()
    {
        displayString = readPhrase;
        readText.color = Color.white;
        readText.text = displayString;
        wordCount = 0;
    }
}
/*
//backup
public class PhraseRecognition : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Unity UI Text component used to post text results on screen.")]
    public Text readText;
    [Tooltip("The text that the user has to read")]
    public string readPhrase;
    private string[] readStrings;

    public string[] words;     //Segmentation in words of the phrase to be read
                               //Created automatically on awake
                               //Accessed by SpeechRecognition.CreatePhraseList()

    [Header("Font Format Settings")]
    [Tooltip("Color for incorrect read words, in string format (using HTML)")]
    [SerializeField] private string correctColor = "cyan";
    [Tooltip("Color for correct read words, in string format (using HTML)")]
    [SerializeField] private string incorrectColor = "red";
    [Tooltip("Color for unread words, in string format (using HTML)")]
    [SerializeField] private string unreadColor = "white";

    [Tooltip("Font color for current word")]
    string color;     //string for dynamic color,
                      //to hold a variable color (correct or incorrect)
                      //according to SpeechRecognition & AssessmentResult
    [Tooltip("Font size for current word")]
    [SerializeField] private int fontSize = 42;

    [Tooltip("Index # of the current word within the phrase to be recognized")]
    public int wordCount = 0;

    string displayString;    //string to hold text since it needs to run on update
                             //to change format on screen.

    //Events
    public delegate void PhraseRecognized();
    public event PhraseRecognized OnPhraseRecognized;

    void Awake()
    {
        CreateStringPhrases();
        SetText();
        AddKeywords();
    }

    private void Start()
    {
        StartCoroutine(FinishedPhrase());
        CultureInfo.CurrentCulture = new CultureInfo("es-AR");
        Debug.Log("The current culture is {0}.\n" +
                        System.Globalization.CultureInfo.CurrentCulture.Name);

        SpeechRecognition.Instance.recognizer.Recognizing += PronunciationAssessment; 
    }

    private void Update()
    {
        readText.text = displayString;
    }


    private void OnDisable()
    {
        RemoveKeywords();

        StopCoroutine(FinishedPhrase());
    }

    /// <summary>
    /// Check if the recognized words match the phrase to be read.
    /// Run from SpeechRecognition on Update
    /// </summary>
    /// <param name="recognizedPhrase"></param>
    public void PronunciationAssessment(object sender, SpeechRecognitionEventArgs e)
    {
        string recognizedPhrase = "";

        if (e.Result.Reason == ResultReason.RecognizingSpeech)
        {
            recognizedPhrase = e.Result.Text;
        }

        string[] recognizedWords = recognizedPhrase.Split();

        for (int i = 0; i < recognizedWords.Length; i++)
        {
            //if (recognizedWords[i].Equals(words[wordCount], StringComparison.CurrentCultureIgnoreCase))
            //String Equality does not ignore accent
            //Using String Comparison instead
            if (string.Compare(words[wordCount], recognizedWords[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
            {
                CreateStringPhrase(wordCount, correctColor);
                displayString = readStrings[wordCount];
                wordCount++;
            }
            else
            {
                CreateStringPhrase(wordCount, incorrectColor);
                displayString = readStrings[wordCount];
            }
        }
    }

    /// <summary>
    /// Set ending display text. Run from SpeechRecognition once assessment finished and result was correct.
    /// </summary>
    public void FinishAssessment()
    {
        displayString = readPhrase;
        readText.color = Color.cyan;
        readText.text = displayString;
        SpeechRecognition.Instance.recognizer.Recognizing -= PronunciationAssessment;
        StopCoroutine(FinishedPhrase());
    }

    /// <summary>
    /// Automatical karaoke-like effect for a given phrase
    /// Creates different string highlighting each word in the phrase to be read
    /// Allowing to switch between the word that is currently read in real time
    /// using Speech & Phrase Recognition
    /// Each string is stored in phrases array.
    /// </summary>
    void CreateStringPhrases()
    {
        string col1 = "<color=" + unreadColor + ">";
        string col2 = "<color=" + correctColor + ">";
        string dynamicColor = "<color=" + color + ">";
        string colorOut = "</color>";
        string bold = "<b>";
        string size = "<size=" + fontSize + ">";
        string closeFormat = "</size></b></color>";

        words = readPhrase.Split();

        readStrings = new string[words.Length];
        Debug.Log("Amount of words in phrase= " + readStrings.Length);

        //Repeats process for each word in the phrase
        for (int phrase = 0; phrase < readStrings.Length; phrase++)
        {
            //create a string for the phrase
            string phraseString = "";

            //Add words that were read in cyan color format
            //open cyan color format
            phraseString += col2;
            //Add read words;
            for (int word = 0; word < words.Length; word++)
            {
                if (word < phrase)
                {
                    phraseString += words[word] + " ";
                }
            }
            //close cyan color format
            phraseString += colorOut;

            //add current word in highlighted format
            phraseString += dynamicColor + bold + size + words[phrase] + closeFormat + " ";

            //Add words that were not read yet in white color format
            //open white color format
            phraseString += col1;
            //add unread words
            for (int word = 0; word < words.Length; word++)
            {
                if (word > phrase)
                {
                    phraseString += words[word] + " ";
                }
            }
            //close white color format
            phraseString += colorOut;

            //Debug.Log("Phrase" + phrase + ". Generated String=" + phraseString);
            // Set phrase to the generated string
            readStrings[phrase] = phraseString;
        }
    }

    /// <summary>
    /// Creates String for phrase to be displayed in a dynamic color
    /// according to the result of Speech & Phrase Recognition.
    /// </summary>
    /// <param name="phrase"></param>
    /// <param name="color"></param>
    void CreateStringPhrase(int phrase, string color)
    {
        string col1 = "<color=" + unreadColor + ">";
        string col2 = "<color=" + correctColor + ">";
        string dynamicColor = "<color=" + color + ">";
        string colorOut = "</color>";
        string bold = "<b>";
        string size = "<size=" + fontSize + ">";
        string closeFormat = "</size></b></color>";

        string phraseString = "";
        // Add read words;
        phraseString += col2;
        for (int word = 0; word < words.Length; word++)
        {
            if (word < phrase)
            {
                phraseString += words[word] + " ";
            }
        }
        phraseString += colorOut;
        // add current word in highlighted format
        phraseString += dynamicColor + bold + size + words[phrase] + closeFormat + " ";
        // add unread words
        phraseString += col1;
        for (int word = 0; word < words.Length; word++)
        {
            if (word > phrase)
            {
                phraseString += words[word] + " ";
            }
        }
        phraseString += colorOut;

        //set phrase to the generated string
        readStrings[phrase] = phraseString;
    }

    /// <summary>
    /// Stops Recognizer, Assesment and Fire Event when phrase is well-read.
    /// </summary>
    /// <returns></returns>
    IEnumerator FinishedPhrase()
    {
        yield return new WaitUntil(()=> wordCount == words.Length);
        Debug.Log("All keywords are recognized.");
        FinishAssessment();
        SpeechRecognition.Instance.recognizer.Recognizing -= PronunciationAssessment;
        SpeechRecognition.Instance.StopRecognition();
        OnPhraseRecognized.Invoke();
    }

    public void SetText()
    {
        displayString = readPhrase;
        readText.color = Color.white;
        readText.text = displayString;
        wordCount = 0;
    }

    /// <summary>
    /// Add keywords to Recognizers Phrase List. 
    /// Make sure to dispose after use.
    /// </summary>
    void AddKeywords()
    {
        foreach (var keyword in words)
        {
            SpeechRecognition.Instance.PhraseList.Add(keyword);
        }
    }

    /// <summary>
    /// Remove keywords from Recognizers Phrase List.
    /// </summary>
    void RemoveKeywords()
    {
        foreach (var keyword in words)
        {
            SpeechRecognition.Instance.PhraseList.Remove(keyword);
        }
    }
}
*/