using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoskResultText : MonoBehaviour
{
    public VoskSpeechToText VoskSpeechToText;
    [SerializeField] private TextMeshProUGUI ResultText;

    private string previousResults;
    private string finalResult;
    private string partialResult;

    [SerializeField] private Color partialColor;
    [SerializeField] private Color finalColor;

    void Awake()
    {
        ResultText.text = "";
        VoskSpeechToText.OnTranscriptionPartialResult += DisplayPartialText;
        VoskSpeechToText.OnTranscriptionResult += DisplayFinalText;
    }

    private void Update()
    {
        ResultText.text = finalResult + " " + partialResult + " ";
    }

    private void DisplayPartialText(string SpeechToText)
    {
        var result = new RecognitionResult(SpeechToText);
        string newPartialResult = result.Phrases[0].Text; // Get only first alternative. No need to iterate through alternatives.

        Debug.Log("PartialResult=" + result.Phrases[0].Text + " / " + result.Partial);

        partialResult = AddRichTextTags(newPartialResult, partialColor);
    }

    private void DisplayFinalText(string SpeechToText)
    {
        var result = new RecognitionResult(SpeechToText);
        string resultString = result.Phrases[0].Text;  // Get only first alternative. No need to iterate through alternatives.

        Debug.Log("FinalResult=" + result.Phrases[0].Text + " / " + result.Partial);

        previousResults = previousResults + " " + resultString; // Add new text result to previous text results
        finalResult = AddRichTextTags(previousResults, finalColor); // Display recognized text result
    }

    public string AddRichTextTags(string text, Color color)
    {
        string openTag = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">";
        string closeTag = "</color>";

        string formattedString = openTag + text + closeTag;

        return formattedString;
    }
}
