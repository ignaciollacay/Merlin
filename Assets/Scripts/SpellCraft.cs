using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// FIXME
// SpellCraft
// Used to craft Spells

public class SpellCraft : MonoBehaviour //Spell
{
    [Header("Spell")]
    public SpellSO spellSO;
    public Text text;

    // Event
    //public delegate void SpellCasted();
    //public event SpellCasted OnSpellCast;

    public PhraseRecognition phraseRecognition;
    [SerializeField] CraftManager craftManager;

    // Helps to display text in editor
    //private void OnValidate()
    //{
    //    if (spellSO == null)
    //    {
    //        text.text = "";
    //        phraseRecognition.readPhrase = "";
    //    }
    //    else
    //    {
    //        SetSpell();
    //    }
    //}

    // Sets components

    public void Awake()
    {
        text = GetComponent<Text>();
        //text.text = spellSO.spellName + ": " + spellSO.Spell;
        phraseRecognition = GetComponent<PhraseRecognition>();
        phraseRecognition.textComponent = text;
    }

    // Event...
    //private void Start()
    //{
    //    phraseRecognition.OnPhraseRecognized += CraftItem;
    //}

    // Run from CraftManager
    public void SetSpell()
    {
        phraseRecognition.readPhrase = spellSO.Spell;
        phraseRecognition.AddPhrase();
        phraseRecognition.OnPhraseRecognized += CraftItem;
    }

    public void ResetSpell()
    {
        spellSO = null;
        text.text = ""; //no es managed por el phrase rec on update?
        phraseRecognition.RemovePhrase();
    }

    public void CraftItem()
    {
        phraseRecognition.OnPhraseRecognized -= CraftItem;
        craftManager.CraftSpell(spellSO.result);
        //OnSpellCast.Invoke();
    }

}
