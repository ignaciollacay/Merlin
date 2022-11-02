using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public SpellSO spellSO;

    public static InventorySlot Create(SpellSO spell, GameObject slotPrefab, Transform inventoryUI)
    {
        GameObject obj = Instantiate(slotPrefab, inventoryUI);
        InventorySlot slot = obj.GetComponent<InventorySlot>();
        slot.spellSO = spell;
        obj.GetComponent<Image>().sprite = spell.icon;

        return slot;
    }
}
