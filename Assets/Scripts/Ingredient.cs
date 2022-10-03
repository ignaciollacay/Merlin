using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Very similar to Spell
// Can be refactored with inheritance
// Replaces SO type and Selection method

[RequireComponent(typeof(Text), typeof(PhraseRecognition))]
public class Ingredient : MonoBehaviour
{
    public ItemSO item;

    private Text text;
    private PhraseRecognition phraseRecognition;
    [SerializeField] private CraftManager craftManager;

    // Helps to display text in editor
    private void OnValidate()
    {
        SetName();
    }

    // Creo que setteando en el editor ya es suficiente, pero por las dudas
    private void Awake()
    {
        SetName();

        //craftManager = CraftManager.Instance;
    }

    private void Start()
    {
        phraseRecognition.OnPhraseRecognized += SelectItem;
    }

    public void SelectItem() // Make private. Only for Editor Testing with button instead of speech.
    {
        craftManager.ItemsSelected(item);
    }

    private void SetName()
    {
        text = GetComponent<Text>();
        text.text = item.itemName;

        phraseRecognition = GetComponent<PhraseRecognition>();
        phraseRecognition.readPhrase = item.itemName;
        phraseRecognition.textComponent = text;
    }

    // Remove ingredient from List
    // Add new ingredient to list?
}
