using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Recieves a phrase event. Gets from SpellList the current Spell & Sends an event
public class CastManager : MonoBehaviour
{
    public delegate void SpellCasted(SpellSO spell);
    public event SpellCasted OnSpellCasted;

    public UnityEvent<SpellSO> OnSpellCast;
    [SerializeField] private AssessmentManager assessmentHandler;
    private SpellSO spell;

    void Start()
    {
        FindObjectOfType<PhraseRecognition>().OnPhraseRecognition.AddListener(CastSpell);
    }

    // Run OnPhraseRecognition
    public void CastSpell()
    {
        OnSpellCast?.Invoke(spell);
    }

    // Run OnAssignmentStart
    public void UpdateSpell(SpellSO _spell)
    {
        spell = _spell;
    }
}
