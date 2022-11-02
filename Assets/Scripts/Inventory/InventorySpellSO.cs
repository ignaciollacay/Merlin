using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can be used for Spell Learned (ex discovered) and Spell Assignments (ex undiscovered)
[CreateAssetMenu(fileName = "InventorySpells_new", menuName = "SO/Inventory/Spells")]
public class InventorySpellSO : ScriptableObject
{
    public List<SpellSO> spells;

    // TODO: Copie estas funciones en InventorySpells, creo que van ahi. Hay que definir si es mejor alla y sacarlas de aca, refactorizando las referencias.
    public int GetCount() => spells.Count;
    public List<SpellSO> GetList() => spells;
    public void AddItem(SpellSO spell) => spells.Add(spell);
}

/* Generic inventory class with inheritance for both Spells & Dialogues
public class InventorySpellSO2 : Inventory<InventorySpellSO> 
{ 

}
public class InventoryDialogueSO2 : Inventory<InventoryDialogueSO>
{

}

public class Inventory<T> : ScriptableObject where T : ScriptableObject
{
    public List<T> list;
}
*/