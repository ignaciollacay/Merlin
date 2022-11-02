using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySpellSO inventory;
    [SerializeField] private GameObject slotPrefab;

    public List<GameObject> artConfigOnly; // TODO: REMOVE AFTER ART FINISHED! 

    private void Awake()
    {
        foreach (var item in artConfigOnly)
        {
            Destroy(item);
        } // TODO: REMOVE AFTER ART FINISHED! 

        foreach (SpellSO spell in inventory.spells)
        {
            AddSlot(spell);
        }
    }
    public void AddSlot(SpellSO spell)
    {
        InventorySlot.Create(spell, slotPrefab, transform);
    }

}
