using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[DefaultExecutionOrder(-500)] // Needs to be executed after level manager sets references
public class AssessmentManager : Singleton<AssessmentManager>
{
    public InventorySpells assigned;
    public InventorySpells learned;
    //private SpellSO currentAssignment;

    public UnityEvent<SpellSO> OnAssignmentStart;
    public UnityEvent<SpellSO> OnAssignmentContinue;
    public UnityEvent OnAssignmentEnd;


    private void Start()
    {
        StartAssignment();
    }

    public void StartAssignment()
    {
        OnAssignmentStart?.Invoke(assigned.GetCurrentSpell()); //Requires LevelManager to set the inventory before.
    }

    private void NextAssignment()
    {
        OnAssignmentContinue?.Invoke(assigned.GetNextSpell());
    }

    // Change Scene after dialogue has ended & Fires AssignmentFinished Event.
    private void EndAssignment()
    {
        OnAssignmentEnd?.Invoke();
    }

    /// <summary>
    /// Checks if all assigned spells are learned. Run by UnityEvent each time a spell is casted/learned
    /// </summary>
    public void EvaluateAssignment()
    {
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

        if (learnedSpells >= assigned.GetCount())
            EndAssignment();
        else
            NextAssignment();
    }

    public void SetAssignment(InventorySpellSO spellInventory)
    {
        assigned.SetInventory(spellInventory);
    }
}