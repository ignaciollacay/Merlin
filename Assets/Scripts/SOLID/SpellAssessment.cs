using UnityEngine;

[RequireComponent(typeof(SpeechToTextUI))]
public class SpellAssessment : MonoBehaviour
{
    private SpeechToTextUI speechToTextUI;

    private void Awake()
    {
        speechToTextUI = GetComponent<SpeechToTextUI>();
    }

    public void StartSpellAssessment(SpellSO assignedSpell)
    {
        string textToRead = GetTextToReadFromSpell(assignedSpell);
        speechToTextUI.StartNewSpeechToTextAssessment(textToRead);
    }

    private string GetTextToReadFromSpell(SpellSO spellSO)
    {
        return spellSO.Spell;
    }
}
