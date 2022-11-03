using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Rename to AssignmentSpell
public class AssessmentHandler : MonoBehaviour
{
    public InventorySpells assigned;
    public InventorySpells learned;

    public UnityEvent<SpellSO> OnAssignmentStart;
    public UnityEvent OnAssignmentEnd;

    private SpellSO currentAssignment;

    private void Start()
    {
        StartAssignment();
    }

    public void StartAssignment()
    {
        currentAssignment = assigned.GetCurrentSpell();
        OnAssignmentStart?.Invoke(currentAssignment);
        Assignment.Instance.currentAssignment = currentAssignment;
}

    private void NextAssignment()
    {
        currentAssignment = assigned.GetNextSpell();
        OnAssignmentStart?.Invoke(currentAssignment);
    }

    /// <summary>
    /// Checks if all assigned spells are learned. Run by UnityEvent each time a spell is casted/learned
    /// </summary>
    public void EvaluateAssignment()
    {
        int currentCount = 0;
        foreach (var assign in assigned.GetList())
        {
            //TODO: Replace with learned.Find(assignedSpell) ?
            foreach (var learn in learned.GetList())
            {
                if (assign == learn)
                {
                    currentCount++;
                }
            }
        }

        if (currentCount == assigned.GetCount())
            EndAssignment();
        else
            NextAssignment();
    }

    // Change Scene after dialogue has ended & Fires AssignmentFinished Event.
    private void EndAssignment()
    {
        print("Assignment fullfilled, time for battle. \n Add LoadScene OnButtonClick of DialogueEventManager");
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        DialogueManager.Instance.button.onClick.AddListener(sceneHandler.LoadAsync);

        OnAssignmentEnd?.Invoke(); // TODO: Replace functionality with event? 
    }
}