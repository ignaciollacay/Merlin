using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySpellSO inventory;
    [SerializeField] private GameObject slotPrefab;
    private List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        // TODO: Call from Assignment Start? Not used on Battle currently.
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
