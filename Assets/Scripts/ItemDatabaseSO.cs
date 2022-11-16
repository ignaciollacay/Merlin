using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "SO/ItemDatabaseSO")]
public class ItemDatabaseSO : ScriptableObject
{
    public ItemSO[] Items;
    public SpellSO[] Spells;

    // Recieves two input item ingredients and returns output item for craft result or spell
    public ItemSO GetItem(ItemSO itemMixSO0, ItemSO itemMixSO1)
    {
        ItemSO get = null;

        foreach (ItemSO item in Items)
        {
            if (item.type != ItemType.Crafted)
                continue;

            foreach (ItemSO craftable in item.craftables)
            {
                if (craftable == itemMixSO0)
                {
                    get = item;
                }
                else if (craftable == itemMixSO1)
                {
                    get = item;
                }
                else
                {
                    get = null;
                    break;
                }
            }
        }
        Debug.Log("GetItem=" + get);
        return get;
    }

    public SpellSO GetSpell(ItemSO itemMixSO0, ItemSO itemMixSO1)
    {
        SpellSO get = null;

        //Debug.Log("itemMixSO0=" + itemMixSO0);
        //Debug.Log("itemMixSO1=" + itemMixSO1);

        foreach (SpellSO spell in Spells)
        {
            int matchCount = 0;

            foreach (ItemSO ingredient in spell.ingredients)
            {
                //Debug.Log("ingredient=" + ingredient);
                if (ingredient == itemMixSO0)
                {
                    get = spell;
                    matchCount++;
                }
                else if (ingredient == itemMixSO1)
                {
                    get = spell;
                    matchCount++;
                }
                else
                {
                    get = null;
                    break;
                }
            }

            if (matchCount == 2)
                break;
        }

        Debug.Log("GetSpell=" + get);
        return get;
    }

}
