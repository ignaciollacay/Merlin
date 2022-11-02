using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellUI : MonoBehaviour
{
    [SerializeField] private InventorySpells inventorySpells;


    [SerializeField] private CastManager castManager;

    [HideInInspector] public SpellSO spellSO;
    [HideInInspector] public Text text;
    [HideInInspector] public PhraseRecognition phraseRecognition;


    // Sets components
    public void Awake()
    {
        text = GetComponent<Text>();
        phraseRecognition = GetComponent<PhraseRecognition>();
        phraseRecognition.textComponent = text;

        spellSO = inventorySpells.GetCurrentSpell();
    }

    public void SetSpell()
    {
        phraseRecognition.readPhrase = spellSO.Spell;
        phraseRecognition.AddPhrase();
        //phraseRecognition.OnPhraseRecognized += CastSpell;
    }

    public void ResetSpell()
    {
        spellSO = null;
        text.text = ""; //no es managed por el phrase rec on update?
        phraseRecognition.RemovePhrase();
    }
}