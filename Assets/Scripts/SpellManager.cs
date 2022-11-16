using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tracking down similarities between Craft & Cast Managers
// Not Yet Implemented (unknown implementations & uses. Waiting to finish)
public abstract class SpellManager : MonoBehaviour
{
    [Header("References")]
    public ItemDatabaseSO itemDatabaseSO;
    public SpellCraft spellDisplay;

    private void Start()
    {
        
    }

    public abstract void DisplaySpell();

    public abstract void OnSelection(); // CastManager.SpellCasted == CraftManager.ItemCrafted;
}
