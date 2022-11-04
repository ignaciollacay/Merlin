using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Rename to AssignmentSpell
public class AssessmentHandler : MonoBehaviour
{
    public InventorySpells assigned;
    public InventorySpells learned;
    //private SpellSO currentAssignment;

    public UnityEvent<SpellSO> OnAssignmentStart;
    public UnityEvent<SpellSO> OnAssignmentContinue;
    public UnityEvent OnAssignmentEnd;


    private void Start() => StartAssignment();

    public void StartAssignment()
    {
        //currentAssignment = assigned.GetCurrentSpell();
        OnAssignmentStart?.Invoke(assigned.GetCurrentSpell());
        print("New assignment started");
    }

    private void NextAssignment()
    {
        //currentAssignment = assigned.GetNextSpell();
        OnAssignmentStart?.Invoke(assigned.GetNextSpell());
        print("Assigned spells remain. Assigning next spell");
    }

    /// <summary>
    /// Checks if all assigned spells are learned. Run by UnityEvent each time a spell is casted/learned
    /// </summary>
    public void EvaluateAssignment()
    {
        print("Assigned spell done.");
        int learnedSpells = 0;
        foreach (var assign in assigned.GetList())
        {
            //TODO: Replace with learned.Find(assignedSpell) ?
            foreach (var learn in learned.GetList())
            {
                if (assign == learn)
                {
                    learnedSpells++;
                }
            }
        }

        if (learnedSpells == assigned.GetCount())
            EndAssignment();
        else
            NextAssignment();
    }

    // Change Scene after dialogue has ended & Fires AssignmentFinished Event.
    private void EndAssignment()
    {
        print("Assignment fullfilled, time for battle.");
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        //DialogueManager.Instance.button.onClick.AddListener(sceneHandler.LoadAsync); // TODO: Could hold a reference to a button to add, and remove button from dialogue manager
                                                                                     //FIXME: Should be listening to DialogueManager.DialogueEnd()
        DialogueManager.Instance.DialogueEnd.AddListener(sceneHandler.LoadAsync);
        OnAssignmentEnd?.Invoke(); // TODO: Replace functionality with event? 
    }
}