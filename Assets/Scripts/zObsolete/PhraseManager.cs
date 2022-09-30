using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// FIXME Acá deberia estar toda la data fija/estática del PhraseRecognition que se repite
            // Pronunciation assessment method
            // Create String Phrase/s method
            // guardar la info generada en el phrase?

         // De lo contrario, puedo eliminar esta clase reemplazando por un instance del SpeechRecognition
            // Pero creo que la necesito para diferenciar cada proceso y separar funciones
            // por ej. cuando hay multiples frases

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
        CultureInfo.CurrentCulture = new CultureInfo("es-AR");
        //Debug.Log("The current culture is {0}.\n" + CultureInfo.CurrentCulture.Name);
    }
}
