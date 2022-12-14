using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Counter))] // TODO Is this really required or an extension?
public class InventorySpells : MonoBehaviour // Use SO instead?
{
    [SerializeField] private InventorySpellSO inventory;
    public Counter counter;


    private void Awake() => counter = GetComponent<Counter>();

    public SpellSO GetCurrentSpell() => inventory.spells[counter.Count];
    public SpellSO GetNextSpell() => inventory.spells[counter.Next(inventory.spells.Count)];
    public SpellSO GetPreviousSpell() => inventory.spells[counter.Next(inventory.spells.Count)];

    public int GetCount() => inventory.spells.Count;
    public List<SpellSO> GetList() => inventory.spells;
    public void AddItem(SpellSO spell) => inventory.spells.Add(spell);

    public SpellSO FindItem(SpellSO spell)
    {
        foreach (SpellSO _spell in inventory.spells)
        {
            if (spell = _spell)
            {
                return _spell;
            }
        }

        return null;
    }

    public void ResetInventory()
    {
        inventory.spells = new List<SpellSO>();
        counter.Count = 0;
    }

    public void SetInventory(InventorySpellSO inventorySpellSO)
    {
        inventory = inventorySpellSO;
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        if (this.CompareTag("Player"))
            ResetInventory();
    }
#endif
}
