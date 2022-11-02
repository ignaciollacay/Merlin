using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME: Weird class to follow Pet approach. Both should be refactored. 
public class Narrator : MonoBehaviour
{
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
        DialogueManager.Instance.StartDialogue(dialogues[0]);
    }
}
