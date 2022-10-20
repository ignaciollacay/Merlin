using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet : MonoBehaviour
{
    [Header("Pet UI")]
    [SerializeField] private Text messageText;
    [SerializeField] private Text helpText;

    [Header("Dialog")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private List<DialogueSO> dialogues;

    [Tooltip ("Interactive object being animated with spellcast")]
    [SerializeField] ItemSpells item;


    private void Start()
    {
        if (item)
            item.OnSpellLearned += DiscoveredSpell;
    }

    public void DiscoveredSpell(SpellSO spellSO)
    {
        dialogueManager.StartDialogue(dialogues[0]);
    }

    // Run from testing
    public void TutorialIntro()
    {
        dialogueManager.StartDialogue(dialogues[1]);
    }
    // Run from testing
    public void FirstSpellsIntro()
    {
        dialogueManager.StartDialogue(dialogues[2]);
    }
    // Run from testing
    public void FirstBattle()
    {
        dialogueManager.StartDialogue(dialogues[3]);
    }
    // Run from testing
    public void BattleStart()
    {
        dialogueManager.StartDialogue(dialogues[0]);
    }
    // Run from testing
    public void BattleEnd()
    {
        GetComponent<Canvas>().enabled = true;
        dialogueManager.StartDialogue(dialogues[0]);
    }
}