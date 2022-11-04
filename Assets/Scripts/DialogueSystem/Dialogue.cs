using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogueSO; // public? Do I want to change/assign it by code?
    [SerializeField] private bool playOnAwake;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueSO); // FIXME: Trigger by event on Spell Learned. 
                                                            // TODO: How do I define the dialogue to play? I need a inventory of dialogues.
    }
    public void TriggerDialogue(DialogueSO newDialogue)
    {
        DialogueManager.Instance.StartDialogue(newDialogue); // FIXME: Trigger by event on Spell Learned. 
                                                            // TODO: How do I define the dialogue to play? I need a inventory of dialogues.
    }
    private void Awake()
    {
        if (playOnAwake)
            TriggerDialogue();
    }
}
