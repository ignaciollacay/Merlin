using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySpellSO inventory;
    [SerializeField] private GameObject slotPrefab;
    private List<InventorySlot> slots = new List<InventorySlot>();


    private void Awake()
    {
        foreach (SpellSO spell in inventory.spells)
        {
            AddSlot(spell);
        }
    }

    public void AddSlot(SpellSO spell)
    {
        InventorySlot slot = InventorySlot.Create(spell, slotPrefab, transform);
        slots.Add(slot);
    }
}
