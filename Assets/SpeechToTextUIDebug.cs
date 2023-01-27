using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;


// FIXME: Works, but not doing intended behaviour. Remove RichTextTagging behaviour. Use only HTML Utility?.
public class SpeechToTextUIDebug : MonoBehaviour
{
    //[SerializeField] private VoskDebug voskDebug;
    //[SerializeField] private SpeechToTextUI _speechToTextUI;
    //[SerializeField] private VoskSpeechToText voskSpeechToText;

    //private List<string> _debugStrings = new List<string>();

    ///* copy in */
    //private Color unreadColor = Color.black;
    //private Color partialCorrect = new Color(0, 0.33f, 0);
    //private Color finalCorrect = new Color(0, 0.66f, 0);
    //private Color partialIncorrect = new Color(0.33f, 0, 0);
    //private Color finalIncorrect = new Color(0.66f, 0, 0);
    ///* copy out */

    //private int _finalWordCount;
    //private RichTextWord[] _richTextWords;
    //private List<TextMeshProUGUI> _debugTexts = new List<TextMeshProUGUI>();

    //private string _textToRead;

    //private void Start()
    //{
    //    SetNewAlternativeCount(voskSpeechToText.MaxAlternatives);
    //}
    //private void Update()
    //{
    //    for (int i = 0; i < _debugTexts.Count; i++)
    //    {
    //        _debugTexts[i].text = _debugStrings[i];
    //    }
    //}
    //private void OnEnable()
    //{
    //    voskSpeechToText.OnTranscriptionPartialResult += AnalizeSpeechResult;
    //    voskSpeechToText.OnTranscriptionResult += AnalizeSpeechResult;
    //}
    //private void OnDisable()
    //{
    //    voskSpeechToText.OnTranscriptionPartialResult -= AnalizeSpeechResult;
    //    voskSpeechToText.OnTranscriptionResult -= AnalizeSpeechResult;
    //}

    //public void SetNewAlternativeCount(int count)
    //{
    //    _debugTexts = voskDebug.DebugTexts;
    //    _debugStrings.Clear();

    //    for (int i = 0; i < _debugTexts.Count; i++)
    //    {
    //        string _debugString = "Recognized Phrase " + i;
    //        _debugStrings.Add(_debugString);
    //    }
    //}

    ///* copy in */
    //public void AnalizeSpeechResult(string recognizedSpeech)
    //{
    //    var recognitionResult = new RecognitionResult(recognizedSpeech);

    //    if (string.IsNullOrEmpty(recognitionResult.Phrases[0].Text))
    //        return;

    //    int foundWords = FindInTextToRead(recognitionResult);

    //    if (!recognitionResult.Partial)
    //    {
    //        _finalWordCount = foundWords;
    //    }
    //}
    ///* copy out */

    //private int FindInTextToRead(RecognitionResult recognitionResult)
    //{
    //    for (int i = 0; i < _debugTexts.Count; i++)
    //    {
    //        if (i > recognitionResult.Phrases.Length)
    //            _debugTexts[i].text = "";
    //    }

    //    /* copy in */

    //    ColorTag correctTag;
    //    ColorTag incorrectTag;
    //    if (recognitionResult.Partial)
    //    {
    //        correctTag = ColorTag.PartialCorrect;
    //        incorrectTag = ColorTag.PartialIncorrect;
    //    }
    //    else
    //    {
    //        correctTag = ColorTag.FinalCorrect;
    //        incorrectTag = ColorTag.FinalIncorrect;
    //    }

    //    int wordCount = _finalWordCount; //Wanted to have an internal counter from 0, but can't update word count without final word count

    //    for (int phrase = 0; phrase < recognitionResult.Phrases.Length; phrase++) // for each recognized phrase (alternative)
    //    {
    //        string recognizedPhrase = recognitionResult.Phrases[phrase].Text;
    //        string[] recognizedWords = recognizedPhrase.Split();
    //        for (int word = 0; word < recognizedWords.Length; word++) // for each recognized word
    //        {
    //            string expectedWord = _richTextWords[wordCount].Word;
    //            string recognizedWord = recognizedWords[word];

    //            if (string.Compare(recognizedWord, expectedWord, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
    //            {
    //                Debug.Log("Found match. " + recognizedWord + " - " + expectedWord + " in Phrase " + phrase);

    //                // innecesario updatea del displayedText en final results, pero si para partial.
    //                _richTextWords[wordCount].ColorTag = correctTag;
    //                _richTextWords[wordCount].TaggedWord = GetRichTextWord(_richTextWords[wordCount]);
    //                _debugStrings[phrase] = GetRichTextPhrase();

    //                wordCount++;

    //                // Stop searching if all expected words have been found
    //                if (wordCount >= _richTextWords.Length || // if all expected words were recognized
    //                    word >= (recognizedWords.Length - 1)) // if all words in read phrase were correct. Want to return before ending automatically the loop, otherwise it will reach incorrect code lines.
    //                {
    //                    return wordCount;
    //                }
    //                // in first result continue search in next recognized word
    //                else if (phrase == 0)
    //                {
    //                    continue;
    //                }
    //                // in alternatives continue search in next recognized phrase
    //                else
    //                {
    //                    phrase = -1; // Loop iterator will add +1
    //                    break;
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("No match for " + expectedWord + " in Word " + word + " of Phrase " + phrase + ": " + recognizedPhrase);
    //                // in first result continue search in next recognized phrase
    //                if (phrase == 0)
    //                {
    //                    break;
    //                }
    //                // in alternative continue search in next recognized word.
    //                else
    //                {
    //                    continue;
    //                }
    //            }
    //        }
    //    }
    //    Debug.Log("Couldn't find " + _richTextWords[wordCount].Word + " in any of the " + recognitionResult.Phrases.Length + " recognized phrases");
    //    _richTextWords[wordCount].ColorTag = incorrectTag;
    //    _richTextWords[wordCount].TaggedWord = GetRichTextWord(_richTextWords[wordCount]);
    //    _debugStrings[_debugStrings.Count] = GetRichTextPhrase();
    //    return wordCount;
    //}


    ///* copy in */
    //public void StartNewSpeechToTextAssessment()
    //{
    //    _finalWordCount = _speechToTextUI.FinalWordCount;
    //    _richTextWords = _speechToTextUI.RichTextWords;
    //}
    //public string GetRichTextWord(RichTextWord richTextWord)
    //{
    //    switch (richTextWord.ColorTag)
    //    {
    //        case ColorTag.Unread:
    //            return HtmlUtility.ToColor(richTextWord.Word, unreadColor);
    //        case ColorTag.PartialCorrect:
    //            return HtmlUtility.ToColor(richTextWord.Word, partialCorrect);
    //        case ColorTag.PartialIncorrect:
    //            return HtmlUtility.ToColor(richTextWord.Word, partialIncorrect);
    //        case ColorTag.FinalCorrect:
    //            return HtmlUtility.ToColor(richTextWord.Word, finalCorrect);
    //        case ColorTag.FinalIncorrect:
    //            return HtmlUtility.ToColor(richTextWord.Word, finalIncorrect);
    //        default:
    //            return null;
    //    }
    //}
    //public string GetRichTextPhrase()
    //{
    //    string phrase = "";

    //    for (int i = 0; i < _richTextWords.Length; i++)
    //    {
    //        if (i < _richTextWords.Length)
    //            phrase += _richTextWords[i].TaggedWord + " ";
    //        else
    //            phrase += _richTextWords[i].TaggedWord;
    //    }

    //    return phrase;
    //}
    //private void GetRichTextWords()
    //{
    //    string[] wordsToRead = _textToRead.Split();
    //    _richTextWords = new RichTextWord[wordsToRead.Length];

    //    for (int i = 0; i < wordsToRead.Length; i++)
    //    {
    //        RichTextWord richTextWord = new RichTextWord();
    //        _richTextWords[i] = richTextWord;
    //        _richTextWords[i].Word = wordsToRead[i];
    //        _richTextWords[i].ColorTag = ColorTag.Unread;
    //        _richTextWords[i].TaggedWord = GetRichTextWord(_richTextWords[i]);
    //        Debug.Log(_richTextWords[i].Word);
    //    }
    //}

    ///* copy out */
}