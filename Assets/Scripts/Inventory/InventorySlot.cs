using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles creation of a inventory slot to add to UI with a public static function. Stores values in the component.
/// </summary>
public class InventorySlot : MonoBehaviour
{
    public SpellSO spellSO;

    /// <summary>
    /// Create slot and assign values corresponding to the given Spell SO.
    /// </summary>
    /// <param name="spell">Spell SO to assign slot icon</param>
    /// <param name="slotPrefab">slot prefab</param>
    /// <param name="inventoryUI">Inventory container where slot will be added as child</param>
    /// <returns>Created Slot</returns>
    public static InventorySlot Create(SpellSO spell, GameObject slotPrefab, Transform inventoryUI)
    {
        GameObject obj = Instantiate(slotPrefab, inventoryUI);
        InventorySlot slot = obj.GetComponent<InventorySlot>();
        slot.spellSO = spell;
        obj.GetComponent<Image>().sprite = spell.icon;

        return slot;
    }
}
