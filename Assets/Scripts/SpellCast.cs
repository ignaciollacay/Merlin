using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCast : Spell
{
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

    public override void Awake()
    {
        text = GetComponent<Text>();
        phraseRecognition = GetComponent<PhraseRecognition>();
        phraseRecognition.textComponent = text;
    }

    //private void Start()
    //{
    //    phraseRecognition.OnPhraseRecognized += CraftItem;
    //}

    // FIXME make private. Made public for debugging with button
    public void CraftItem()
    {
        phraseRecognition.OnPhraseRecognized -= CraftItem;
        craftManager.CraftSpell(spellSO.result);
        //OnSpellCast.Invoke();
    }

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
}
