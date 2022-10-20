using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastManager : MonoBehaviour
{
    [Header("References")]
<<<<<<< Updated upstream
    [SerializeField] private SpellBook spellBook; // The 
    [SerializeField] private SpellCast spellDisplay; // The Spell component in the Magic Scroll UI

    // FIXME REFACTOR
    // Move to SpellCast
=======
    [SerializeField] private SpellBook spellBook;
    [SerializeField] private SpellCast spellDisplay;

>>>>>>> Stashed changes
    public delegate void SpellCasted(SpellSO spellCasted);
    public event SpellCasted OnSpellCasted;

    private void Start()
    {
        DisplaySpell(spellBook.GetSpell());
<<<<<<< Updated upstream

        // FIXME REFACTOR
        #region TODO Refactor OnPhraseRecognized - CastSpell
        // Avoids cross reference & more clear code.
        // Not Implemented to keep code structure between SpellCast & SpellCraft
        //spellDisplay.phraseRecognition.OnPhraseRecognized += CastSpell;
        #endregion
=======
        spellDisplay.phraseRecognition.OnPhraseRecognized += CastSpell;
>>>>>>> Stashed changes
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
        DisplaySpell(spellBook.GetNextSpell());
    }

    // Switch to the previous spell
    public void PreviousSpell()
    {
<<<<<<< Updated upstream
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
=======
        spellDisplay.phraseRecognition.RemovePhrase();
        DisplaySpell(spellBook.GetPreviousSpell());
    }

    public void CastSpell()
    {
        OnSpellCasted?.Invoke(spellDisplay.spellSO);
>>>>>>> Stashed changes
    }
}
