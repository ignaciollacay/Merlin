using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Sets the text to be read
/// Recieves Speech To Text Result
/// Compares the text to be read with Speech To Text result
/// Updates the color of the text with different color for correct or incorrect check
/// </summary>
public class SpeechToTextUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoskSpeechToText voskSpeechToText;
    [SerializeField] private TextMeshProUGUI textGUI;

    [Header("Event")]
    public UnityEvent OnPhraseWellRead;

    protected int readWords;
    protected RichTextPhrase richTextPhrase = new RichTextPhrase("");

    public virtual void Update()
    {
        textGUI.text = richTextPhrase.taggedPhrase;
    }

    private void OnEnable()
    {
        voskSpeechToText.OnTranscriptionPartialResult += AnalizeSpeechResult;
        voskSpeechToText.OnTranscriptionResult += AnalizeSpeechResult;
    }
    private void OnDisable()
    {
        voskSpeechToText.OnTranscriptionPartialResult -= AnalizeSpeechResult;
        voskSpeechToText.OnTranscriptionResult -= AnalizeSpeechResult;
    }

    public virtual void AnalizeSpeechResult(string recognizedSpeech)
    {
        var recognitionResult = new RecognitionResult(recognizedSpeech);

        if (string.IsNullOrEmpty(recognitionResult.Phrases[0].Text))
            return;

        int foundWords = FindInTextToRead(recognitionResult);
        if (!recognitionResult.Partial)
        {
            readWords = foundWords;
        }

        if (readWords >= richTextPhrase.words.Count)
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
    public virtual int FindInTextToRead(RecognitionResult recognitionResult)
    {
        int foundWords = readWords;

        for (int phrase = 0; phrase < recognitionResult.Phrases.Length; phrase++) // for each recognized phrase (alternative)
        {
            string recognizedPhrase = recognitionResult.Phrases[phrase].Text;
            string[] recognizedWords = recognizedPhrase.Split();

            for (int word = 0; word < recognizedWords.Length; word++) // for each recognized word
            {
                string expectedWord = richTextPhrase.words[foundWords].Word;
                string recognizedWord = recognizedWords[word];

                if (string.Compare(recognizedWord, expectedWord, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                {
                    Debug.Log("Found match. " + recognizedWord + " - " + expectedWord + " in Phrase " + phrase);

                    richTextPhrase.words[foundWords].isRead = true;
                    richTextPhrase.words[foundWords].isCorrect = true;
                    richTextPhrase.words[foundWords].isPartial = recognitionResult.Partial;

                    foundWords++;

                    // Stop searching if all expected words have been found
                    if (foundWords >= richTextPhrase.words.Count || // if all expected words were recognized
                        word >= (recognizedWords.Length - 1)) // if all words in read phrase were correct. Want to return before ending automatically the loop, otherwise it will reach incorrect code lines.
                    {
                        return foundWords;
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
        Debug.Log("Couldn't find " + richTextPhrase.words[foundWords].Word + " in any of the " + recognitionResult.Phrases.Length + " recognized phrases");
        richTextPhrase.words[foundWords].isRead = true;
        richTextPhrase.words[foundWords].isCorrect = false;
        richTextPhrase.words[foundWords].isPartial = recognitionResult.Partial;

        return foundWords;
    }

    private void EndSpeechToTextAssessment()
    {
        OnPhraseWellRead?.Invoke();
        richTextPhrase.SetPhraseCorrect();
    }

    public void StartNewSpeechToTextAssessment(string textToRead)
    {
        richTextPhrase = new RichTextPhrase(textToRead);
        readWords = 0;
    }

    // Allow to skip during tests (called on button click) - for development
    public void SkipSpeechToTextAssessment()
    {
        EndSpeechToTextAssessment();
    }
}