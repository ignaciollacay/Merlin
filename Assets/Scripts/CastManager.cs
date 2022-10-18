using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellBook spellBook; // The 
    [SerializeField] private SpellCast spellDisplay; // The Spell component in the Magic Scroll UI

    // FIXME REFACTOR
    // Move to SpellCast
    public delegate void SpellCasted(SpellSO spellCasted);
    public event SpellCasted OnSpellCasted;

    private void Start()
    {
        DisplaySpell(spellBook.GetSpell());

        // FIXME REFACTOR
        #region TODO Refactor OnPhraseRecognized - CastSpell
        // Avoids cross reference & more clear code.
        // Not Implemented to keep code structure between SpellCast & SpellCraft
        //spellDisplay.phraseRecognition.OnPhraseRecognized += CastSpell;
        #endregion
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
        DisplaySpell(spellBook.GetNextSpell());
    }

    // Switch to the previous spell
    public void PreviousSpell()
    {
        DisplaySpell(spellBook.GetPreviousSpell());
    }

    // Run from SpellCast
    // FIXME REFACTOR Can be run from SpellCast. 
    public void CastSpell(SpellSO spell)
    {
        // Spell was casted.
        // Invoke event 
        // Do things by event subscription
        OnSpellCasted?.Invoke(spell);
    }
}
