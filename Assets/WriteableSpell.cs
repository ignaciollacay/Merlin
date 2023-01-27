using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteableSpell : MonoBehaviour
{
    public InventorySpells inventory;

    public void UpdateSpell(string spell)
    {
        inventory.GetCurrentSpell().SetSpell(spell);
    }
}
