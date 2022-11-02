using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleStarter : MonoBehaviour
{
    public InventorySpellSO assigned;
    public InventorySpellSO learned;

    public UnityEvent OnAssignmentFinished;


    // Checks if all assigned spells are learned.
    // Run by UnityEvent each time a spell is casted/learned
    public void EvaluateAssignment()
    {
        int currentCount = 0;
        foreach (var assign in assigned.spells)
        {
            //TODO: Replace with learned.Find(assignedSpell) ?
            foreach (var learn in learned.spells)
            {
                if (assign == learn)
                {
                    currentCount++;
                }
            }
        }

        if (currentCount == assigned.GetCount())
            AssignmentFinished();

        /* // FIXME: (WIP) Replace count check with spell check. More flexible and allow for a persistent learned spell inventory   
        if (learned.GetCount() >= assigned.GetCount())
        {
            AssignmentFinished();
        }
        */
    }

    // Change Scene after dialogue has ended & Fires AssignmentFinished Event.
    private void AssignmentFinished()
    {
        print("Assignment fullfilled, time for battle. \n Add LoadScene OnButtonClick of DialogueEventManager");
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        DialogueManager.Instance.button.onClick.AddListener(sceneHandler.LoadAsync);

        OnAssignmentFinished?.Invoke(); // TODO: Replace functionality with event? 
    }
}
