using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// FIXME
// Actual spellcast script // Brother from SpellCraft (ex SpellCast)
// Used To cast Spells

public class SpellCast : MonoBehaviour //Spell
{
    [Header("Spell")]
    [HideInInspector] public SpellSO spellSO;
    [HideInInspector] public Text text;

    // Event
    //public delegate void SpellCasted();
    //public event SpellCasted OnSpellCast;

    public PhraseRecognition phraseRecognition;
    [SerializeField] CastManager castManager;

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
    private void Start()
    {
        phraseRecognition.OnPhraseRecognized += CastSpell;
    }

    public void SetSpell()
    {
        //text.text managed by PhraseRecognition.AddPhrase() > PhraseRecognition.SetText() > PhraseRecognition.Update()
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

    public void CastSpell()
    {
        //phraseRecognition.OnPhraseRecognized -= CastSpell;
        castManager.CastSpell(spellSO);
        //OnSpellCast.Invoke();
    }

}
