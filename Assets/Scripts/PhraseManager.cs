using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PhraseManager : MonoBehaviour
{
    public static PhraseManager Instance { get; set; }

    public GameObject ingredients;
    public SpeechRecognition speechManager;
    public List<PhraseRecognition> phraseRecs;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var item in phraseRecs)
        {
            speechManager.phraseRecognition.Add(item);
        }

        CultureInfo.CurrentCulture = new CultureInfo("es-AR");
        //Debug.Log("The current culture is {0}.\n" + CultureInfo.CurrentCulture.Name);
    }
}
