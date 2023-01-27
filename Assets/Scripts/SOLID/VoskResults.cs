using TMPro;
using UnityEngine;

public class VoskResults : MonoBehaviour
{
    [SerializeField] private VoskSpeechToText voskSpeechToText;

    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[5];
    private string[] phrases = new string[5];

    private void OnEnable()
    {
        voskSpeechToText.OnTranscriptionPartialResult += GetSpeechResults;
    }

    private void GetSpeechResults(string recognizedSpeech)
    {
        var recognitionResult = new RecognitionResult(recognizedSpeech);
        for (int i = 0; i < recognitionResult.Phrases.Length; i++)
        {
            phrases[i] = recognitionResult.Phrases[i].Text;
        }
    }

    private void Update()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = phrases[i];
        }
    }
}