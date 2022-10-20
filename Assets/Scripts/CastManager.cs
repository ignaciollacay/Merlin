using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellBook spellBook;
    [SerializeField] private SpellCast spellDisplay;

    public delegate void SpellCasted(SpellSO spellCasted);
    public event SpellCasted OnSpellCasted;

    private void Start()
    {
        DisplaySpell(spellBook.GetSpell());
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
        DisplaySpell(spellBook.GetNextSpell());
    }

    // Switch to the previous spell
    public void PreviousSpell()
    {
        spellDisplay.phraseRecognition.RemovePhrase();
        DisplaySpell(spellBook.GetPreviousSpell());
    }

    public void CastSpell()
    {
        OnSpellCasted?.Invoke(spellDisplay.spellSO);
    }
}
