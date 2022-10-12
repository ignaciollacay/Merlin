using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public SpellBook spellBook;
    public List<SpellSO> spellsDiscovered;

    void Awake()
    {
        foreach (var spell in spellsDiscovered)
        {
            spellBook.discoveredSpells.Add(spell);
        }
    }
}
