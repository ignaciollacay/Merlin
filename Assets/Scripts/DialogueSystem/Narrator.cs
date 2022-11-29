using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Narrator : MonoBehaviour
{
    public DialogueManager dialogueManager;
    [Header("Dialog")]
    //[SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private List<DialogueSO> dialogues;

    private void Awake()
    {
        // Safety measure to prevent dialogue from starting on play
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        dialogueManager.StartDialogue(dialogues[0]);
    }
}
