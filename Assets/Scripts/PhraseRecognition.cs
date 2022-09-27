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
        CultureInfo.CurrentCulture = new CultureInfo("es-AR");
        Debug.Log("The current culture is {0}.\n" +
                        System.Globalization.CultureInfo.CurrentCulture.Name);
    }
    private void Update()
    {
        readText.text = displayString;
    }
    private void OnDisable()
    {
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

    IEnumerator FinishedPhrase()
    {
        yield return new WaitUntil(()=> wordCount == words.Length);
        Debug.Log("All keywords are recognized.");
        StopAssessment();
        OnPhraseRecognized.Invoke();
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
public class PhraseRecognition2 : MonoBehaviour
{
    [Header("Recognizer")]
    [SerializeField] private SpeechRecognition speechManager;

    [Header("Text")]
    [SerializeField] private Text displayText;
    [SerializeField] private string readString = "Aqui vengo ante ti a consultar al dios cómo podré encontrar y liberar a Ulises";
    public string[] keywords = new string[15]
    {
        "Aquí",
        "vengo",
        "ante",
        "ti",
        "a",
        "consultar",
        "al",
        "dios",
        "cómo",
        "podré",
        "encontrar",
        "y",
        "liberar",
        "a",
        "Ulises"
    }; //REFACTOR: Create on awake by readString.Split
    string[] displayPhrase = new string[15]
    {
        "<color=cyan><b><size=42>Aqui</size></b></color> vengo ante ti a consultar al dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui <b><size=42>vengo</size></b></color> ante ti a consultar al dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo <b><size=42>ante</size></b></color> ti a consultar al dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante <b><size=42>ti</size></b></color> a consultar al dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti <b><size=42>a</size></b></color> consultar al dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a <b><size=42>consultar</size></b></color> al dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar <b><size=42>al</size></b></color> dios cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al <b><size=42>dios</size></b></color> cómo podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios <b><size=42>cómo</size></b></color> podré encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios cómo <b><size=42>podré</size></b></color> encontrar y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios cómo podré <b><size=42>encontrar</size></b></color> y liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios cómo podré encontrar <b><size=42>y</size></b></color> liberar a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios cómo podré encontrar y <b><size=42>liberar</size></b></color> a Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios cómo podré encontrar y liberar <b><size=42>a</size></b></color> Ulises",
        "<color=cyan>Aqui vengo ante ti a consultar al dios cómo podré encontrar y liberar a <b><size=42>Ulises</size></b></color>"
    };


    [Header("Highlight Properties")]
    [SerializeField] private string colorCorrect = "cyan";
    [SerializeField] private string colorIncorrect = "red";
    [SerializeField] private string colorDefault = "white";
    [SerializeField] private int size = 42;

    [Tooltip("Index # of the current word within the phrase to be recognized")]
    public int wordCount = 0;

    void Awake()
    {
        // Set starting display text to be read
        displayText.text = readString;
    }

    /// <summary>
    /// Check if the recognized words match the phrase to be read.
    /// Run from SpeechRecognition on Update
    /// </summary>
    /// <param name="recognizedPhrase"></param>
    public void PronunciationAssessment(string recognizedPhrase)
    {
        if (wordCount < keywords.Length)
        {
            string[] recognizedWords = recognizedPhrase.Split();

            for (int i = 0; i < recognizedWords.Length; i++)
            {
                if (recognizedWords[i].Equals(keywords[wordCount], System.StringComparison.CurrentCultureIgnoreCase))
                {
                    displayText.text = displayPhrase[wordCount];
                    //displayText.text = PhraseColor(wordCount, colorCorrect);
                    wordCount++;
                }
                else if (recognizedPhrase != "")
                {
                    displayText.text = PhraseColor(wordCount, colorDefault);
                }
            }
        }
        else
        {
            Debug.LogWarning("Index error. All keywords are recognized. Disable Speech Recognition & Assessment");
        }
    }

    /// <summary>
    /// Set ending display text. Run from SpeechRecognition once assessment finished and result was correct.
    /// </summary>
    public void StopAssessment()
    {
        displayText.text = readString;
        displayText.color = Color.cyan;
    }

    // Display phrases with dynamic color
    // Para mostrar en rojo cuando dice incorrecto, pero pisa el default antes de hablar.
    private string PhraseColor(int wordCount, string color)
    {
        displayPhrase = new string[15]
        {
            "<color="+color+"><b><size=42>Aqui</size></b></color> vengo ante ti a consultar al dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui</color> <color="+color+"><b><size=42>vengo</size></b></color> ante ti a consultar al dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo</color> <color="+color+"><b><size=42>ante</size></b></color> ti a consultar al dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante</color> <color="+color+"><b><size=42>ti</size></b></color> a consultar al dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti</color> <color="+color+"><b><size=42>a</size></b></color> consultar al dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a</color> <color="+color+"><b><size=42>consultar</size></b></color> al dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar</color> <color="+color+"><b><size=42>al</size></b></color> dios cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al</color> <color="+color+"><b><size=42>dios</size></b></color> cómo podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios</color> <color="+color+"><b><size=42>cómo</size></b></color> podré encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios cómo</color> <color="+color+"><b><size=42>podré</size></b></color> encontrar y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios cómo podré</color> <color="+color+"><b><size=42>encontrar</size></b></color> y liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios cómo podré encontrar</color> <color="+color+"><b><size=42>y</size></b></color> liberar a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios cómo podré encontrar y</color> <color="+color+"><b><size=42>liberar</size></b></color> a Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios cómo podré encontrar y liberar</color> <color="+color+"><b><size=42>a</size></b></color> Ulises",
            "<color="+colorCorrect+">Aqui vengo ante ti a consultar al dios cómo podré encontrar y liberar a</color> <color="+color+"><b><size=42>Ulises</size></b></color>"
        };
        return displayPhrase[wordCount];
    }
}
*/