using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is now obsolete.
//Generates an array of strings, each with a different word of the given phrase highlighted in a karaoke-like effect
//Integrated in phrase Recognition. 
public class KaraokeGenerator : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Unity UI Text component used to post text results on screen.")]
    public Text readText;
    [Tooltip("The text that the user has to read")]
    public string readPhrase = "Aqui vengo ante ti a consultar al dios cómo podré encontrar y liberar a Ulises";
    string[] phrases;
    string[] words;

    [Header("Font Format Settings")]
    [Tooltip("Color for read words, in string format (using HTML)")]
    [SerializeField] private string ReadColor = "cyan";
    [Tooltip("Color for unread words, in string format (using HTML)")]
    [SerializeField] private string UnreadColor = "white";
    //string for dynamic color,
    //to hold a variable color (correct or incorrect)
    //according to SpeechRecognition & AssessmentResult
    [Tooltip("Font color for current word")]
    string color;
    [Tooltip("Font size for current word")]
    [SerializeField] private int fontSize = 21;

    /// <summary>
    /// Automatical karaoke-like effect for a given phrase
    /// Creates different string highlighting each word in the phrase to be read
    /// Allowing to switch between the word that is currently read in real time
    /// using Speech & Phrase Recognition
    /// Each string is stored in phrases array.
    /// </summary>
    void CreateStringPhrases()
    {
        string col1 = "<color="+ UnreadColor + ">";
        string col2 = "<color="+ ReadColor +">";
        string dynamicColor = "<color=" + color + ">";
        string colorOut = "</color>";
        string bold = "<b>";
        string size = "<size="+ fontSize +">";
        string closeFormat = "</size></b></color>";

        words = readPhrase.Split();

        phrases = new string[words.Length];
        Debug.Log("Amount of words in phrase= " + phrases.Length);

        //Repeats process for each word in the phrase
        for (int phrase = 0; phrase < phrases.Length; phrase++)
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

            Debug.Log("Phrase"+ phrase+ ". Generated String=" + phraseString);
            //set phrase to the generated string
            phrases[phrase] = phraseString;
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
        string col1 = "<color=" + UnreadColor + ">";
        string col2 = "<color=" + ReadColor + ">";
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
        phrases[phrase] = phraseString;
    }
    private void Awake()
    {
        CreateStringPhrases();
    }
    private void Start()
    {
        if (phrases[0] != null)
            readText.text = phrases[0];
        else
            Debug.Log("Error. Phrase is null");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            readText.text = phrases[0];
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            readText.text = phrases[1];
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            readText.text = phrases[2];
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            readText.text = phrases[3];
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            readText.text = phrases[4];
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            readText.text = phrases[5];
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            readText.text = phrases[6];
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            readText.text = phrases[7];
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            readText.text = phrases[8];
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            readText.text = phrases[9];
        }
    }
}
