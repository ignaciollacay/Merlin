using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores reference to multiple instances of dialogues in each scene withing a Singleton Instance
/// Allows to change the dialogueSO reference in each level via the LevelManager.
/// </summary>
public class DialogueManager : Singleton<DialogueManager>
{
    public PetDialogueHandler petDialogue;
    
    public DialogueSO endDialogue { get; set; }


    public void SetPetStartDialogue(DialogueSO dialogue)
    {
        petDialogue.dialogue.dialogueSO = dialogue;
    }

    public void SetEndDialogue(DialogueSO dialogue)
    {
        endDialogue = dialogue;
    }

    public void SetPetEndDialogue()
    {
        petDialogue.dialogue.dialogueSO = endDialogue;
    }
}
