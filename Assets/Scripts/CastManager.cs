using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// FIXME: BROKEN CLASS. NOT SWITCHING BETWEEN SPELLS WITH SCROLL ANYMORE. DECOUPLE UNNECESARY CODE
public class CastManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellBook spellBook;
    [SerializeField] private SpellUI spellDisplay;

    public delegate void SpellCasted(SpellSO spellCasted); // TODO: Replace with UnityEvent
    public event SpellCasted OnSpellCasted; // TODO: Replace with UnityEvent
    public UnityEvent<SpellSO> OnSpellCast;

    private void Start()
    {
        //DisplaySpell(spellBook.GetSpell());
        spellDisplay.phraseRecognition.OnPhraseRecognized += CastSpell;
    }

    private void DisplaySpell(SpellSO currentSpell)
    {
        // Display Spell in UI
        spellDisplay.spellSO = currentSpell;
        spellDisplay.SetSpell();
    }

    // Switch to the next spell
    public void NextSpell()
    {
        spellDisplay.phraseRecognition.RemovePhrase();
        //DisplaySpell(spellBook.GetNextSpell());
    }

    // Switch to the previous spell
    public void PreviousSpell()
    {
        spellDisplay.phraseRecognition.RemovePhrase();
        //DisplaySpell(spellBook.GetPreviousSpell());
    }

    public void CastSpell()
    {
        OnSpellCasted?.Invoke(spellDisplay.spellSO); // TODO: Replace with UnityEvent
        OnSpellCast?.Invoke(spellDisplay.spellSO);
    }
}
