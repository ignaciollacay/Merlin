using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Very similar to Ingredient
// Can be refactored with inheritance
// Replaces SO type and Selection method

[RequireComponent(typeof(Text), typeof(PhraseRecognition))]
public class Spell : MonoBehaviour
{
    // Event
    //public delegate void SpellCasted();
    //public event SpellCasted OnSpellCast;

    public SpellSO spellSO;

    public Text text; // TODO Make private once craft manager OnSpellCraft event works
    public PhraseRecognition phraseRecognition;
    private string newString = "";

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

    private void Awake()
    {
        text = GetComponent<Text>();
        phraseRecognition = GetComponent<PhraseRecognition>();
        phraseRecognition.textComponent = text;
    }

    //private void Start()
    //{
    //    phraseRecognition.OnPhraseRecognized += CraftItem;
    //}

    public void CraftItem() // FIXME make private. Made public for debugging with button
    {
        phraseRecognition.OnPhraseRecognized -= CraftItem;
        CraftManager.Instance.CraftSpell(spellSO.result);
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
