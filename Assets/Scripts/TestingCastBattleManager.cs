using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TestingCastBattleManager : MonoBehaviour
{
#if UNITY_EDITOR
    // Editor Utility. Testing Feature only.
    // Add Spells that are learned on Dojo, which are removed OnApplicationQuit of UnityEditor.
    // OnBuild player already has them on the PlayerInventorySO
    public List<SpellSO> spellsDiscovered;
    public InventorySpells playerInventory;
    void Awake()
    {
        foreach (var spell in spellsDiscovered)
        {
            playerInventory.AddItem(spell);
        }
    }
#endif
}
