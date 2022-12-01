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

    public void SetPetDialogue(DialogueSO dialogue)
    {
        petDialogue.dialogue.dialogueSO = dialogue;
    }
}
