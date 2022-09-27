using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is now obsolete.
//Refactored into PhraseRecognition
public class TextToRead : MonoBehaviour
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
    [SerializeField] string[] displayPhrase = new string[15]
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
    [SerializeField] private int size = 42;

    [Tooltip("Index # of the current word within the phrase to be recognized")]
    public int wordCount = 0;

    public bool correctSpeech = false;

    void Awake()
    {
        // Set starting display text to be read
        SetText(Color.white);
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
                    wordCount++;
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
        SetText(Color.cyan);
        correctSpeech = true;
    }

    public void SetText(Color color)
    {
        displayText.text = readString;
        displayText.color = color;
    }

    // Display phrases with dynamic color
    private string PhraseColor(int phraseNumber, string color)
    {
        displayPhrase = new string[15]
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
        return displayPhrase[phraseNumber];
    }
}