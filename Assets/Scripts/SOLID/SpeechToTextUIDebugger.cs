using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
// Same as Parent class. Extends functionality to 
// 1. Display each recognized result in debug text.
// 2. Display recognized results as RichTextPhrases to highlight where the correct word was found
public class SpeechToTextUIDebugger : SpeechToTextUI
{
    [SerializeField] private VoskDebug voskDebug;
    private List<RichTextPhrase> recognizedPhrases = new();

    public override void Update()
    {
        base.Update();

        for (int i = 0; i < voskDebug._debugTexts.Count; i++)
        {
            if (i < recognizedPhrases.Count)
            {
                voskDebug._debugTexts[i].text = recognizedPhrases[i].taggedPhrase;
            }
        }
    }

    public override int FindInTextToRead(RecognitionResult recognitionResult)
    {
        int foundWords = readWords;
        recognizedPhrases.Clear();

        for (int phrase = 0; phrase < recognitionResult.Phrases.Length; phrase++)
        {
            string recognizedPhrase = recognitionResult.Phrases[phrase].Text;
            string[] recognizedWords = recognizedPhrase.Split();

            // Create a new phrase to display comparison results in corresponding debug text.
            RichTextPhrase newPhrase = new RichTextPhrase(recognizedPhrase);
            recognizedPhrases.Add(newPhrase);

            for (int word = 0; word < recognizedWords.Length; word++)
            {
                string expectedWord = richTextPhrase.words[foundWords].Word;
                string recognizedWord = recognizedWords[word];

                if (string.Compare(recognizedWord, expectedWord, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                {
                    Debug.Log("Found match. " + recognizedWord + " - " + expectedWord + " in Phrase " + phrase);
                    // Set tag to correct
                    newPhrase.words[word].isRead = true;
                    newPhrase.words[word].isCorrect = true;
                    newPhrase.words[word].isPartial = recognitionResult.Partial;

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
                    if (phrase == 0)
                    {
                        break;
                    }
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

    public void StartSpellAssessment(SpellSO assignedSpell)
    {
        string textToRead = GetTextToReadFromSpell(assignedSpell);
        StartNewSpeechToTextAssessment(textToRead);
    }

    private string GetTextToReadFromSpell(SpellSO spellSO)
    {
        return spellSO.Spell;
    }
}