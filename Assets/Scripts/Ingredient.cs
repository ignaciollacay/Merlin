using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(PhraseRecognition))]
public class Ingredient : MonoBehaviour
{
    public ItemSO item;

    private Text text;
    private PhraseRecognition phraseRecognition;

    // Helps to display text in editor
    private void OnValidate()
    {
        SetName();
    }

    // Creo que setteando en el editor ya es suficiente, pero por las dudas
    private void Awake()
    {
        SetName();
    }

    private void Start()
    {
        phraseRecognition.OnPhraseRecognized += SelectItem;
    }

    void SelectItem()
    {
        CraftManager.Instance.ItemsSelected(item);
    }

    void SetName()
    {
        text = GetComponent<Text>();
        text.text = item.Name;

        phraseRecognition = GetComponent<PhraseRecognition>();
        phraseRecognition.readPhrase = item.Name;
        phraseRecognition.readText = text;
    }
}
