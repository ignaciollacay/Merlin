using Microsoft.CognitiveServices.Speech;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// FIXME Acá debería estar solo la parte dinámica del recognition
         // Lo que refiere al text/strings  


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
    public Text textComponent;
    [Tooltip("The text that the user has to read")]
    public string readPhrase;
    public string[] words;           // Segmentation in words of the phrase to be read
                                     // Created automatically on awake
                                     // Accessed by SpeechRecognition.CreatePhraseList()

    private string[] readStrings;    // Array of strings highlighting each word
                                     // using html formatting for karaoke effect.

    private string displayString;    // string to hold text since it needs to run on update
                                     // to change format on screen.

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

    //Events
    public delegate void PhraseRecognized();
    public event PhraseRecognized OnPhraseRecognized;

    private SpeechRecognition speechRecognition;

    [SerializeField] private bool onStartEnabled = true;


    void Awake()
    {
        speechRecognition = SpeechRecognition.Instance;

        if(onStartEnabled)
        {
            CreateStringPhrases();
            SetText();
        }
    }

    private void Start()
    {
        if (onStartEnabled)
        {
            speechRecognition.phraseRecs.Add(this);
            StartCoroutine(PhraseReadCoroutine());
        }
    }

    private void Update()
    {
        textComponent.text = displayString;
    }

    private void OnDisable()
    {
        SpeechRecognition.Instance.phraseRecs.Remove(this);
        StopCoroutine(PhraseReadCoroutine());
    }


    public void SetText()
    {
        displayString = readPhrase;
        textComponent.color = Color.white;
        textComponent.text = displayString;
        wordCount = 0;
    }

    /// <summary>
    /// Runs after assessment finished and result was correct
    /// Set ending display text and remove from SpeechRecognition
    /// </summary>
    public void StopAssessment()
    {
        speechRecognition.StopPhraseRecognition(this);

        displayString = readPhrase;
        textComponent.color = Color.cyan;
        textComponent.text = displayString;

        StopCoroutine(PhraseReadCoroutine());
    }

    private IEnumerator PhraseReadCoroutine()
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

    public void AddPhrase()
    {
        CreateStringPhrases();
        SetText();
        speechRecognition.phraseRecs.Add(this);
        StartCoroutine(PhraseReadCoroutine());
    }

    // TODO Reset everything so that Spell can be read again. Run from CraftManager Spellcrafted event.
    // Needs to clear SpellSO, Words, Text, etc.
    // Opposite to AddPhrase();
    public void RemovePhrase()
    {
        readStrings = new string[0];                    // undos createStringPhrase()
        displayString = "";                             // undos SetText();
        speechRecognition.phraseRecs.Remove(this);      // removes phrase from the list of phrases in Speech Recognizer that subscribe to recognizing event as listeners
    }
}