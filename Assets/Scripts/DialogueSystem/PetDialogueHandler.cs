using UnityEngine;

// TODO: Inherit from Dialogue Handler and replace DialogueHandler duplicate in Canvas_PetDialogue.
// Adds a Dialogue variable to hold reference for the DialogueManager to set  
public class PetDialogueHandler : MonoBehaviour //public class PetDialogueHandler : DialogueHandler
{
    public Dialogue dialogue;

    private void OnValidate()
    {
        if (dialogue == null)
        {
            dialogue = GetComponentInChildren<Dialogue>();

            if (dialogue == null)
            {
                Debug.Log("Could not find dialogue component. Make sure to attach a dialogue component in children. " + name, gameObject);
            }
        }
    }

    private void Awake()
    {
        if (dialogue == null)
        {
            Debug.Log("Missing dialogue reference " + name, gameObject);
        }
    }
}
