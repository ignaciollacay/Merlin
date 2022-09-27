using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    //singleton?
    public static CraftManager Instance { get; set; }

    public ItemDatabaseSO itemDatabaseSO;


    private ItemSO[] combinedItems = new ItemSO[2];

    public void ItemsSelected(ItemSO item)
    {
        if (combinedItems[0] == null)
        {
            //Spawn item
            Instantiate(item.prefab);
            //Add to combined
            combinedItems[0] = item;
        }
        else if (combinedItems[1] == null)
        {
            //Spawn item
            Instantiate(item.prefab);
            //Add to combined
            combinedItems[1] = item;
        }
        else
        {
            //Both items were combined. Craft new item
            Debug.Log("Both items were combined. Crafting new item!");
            CraftItem();
        }
    }

    private void CraftItem()
    {
        ItemSO item = GetItem();

        if (item != null)
        {
            //VFX? SFX? Create item?
            //Instantiate(item.prefab);
            Debug.Log("Crafted=" + item);
        }
        else
        {
            Debug.Log("Crafted item could not be found. Check ItemSO.");
        }
    }

    private ItemSO GetItem()
    {
        foreach (var item in itemDatabaseSO.Items)
        {
            if (item.type != ItemType.Crafted)
            {
                Debug.Log("Skipped(Type)=" + item);
                continue;
            }

            if (item.craftables[0] != combinedItems[0] || item.craftables[0] != combinedItems[1])
            {
                Debug.Log("Skipped(Craftable0)=" + item);
                continue;
            }

            if (item.craftables[1] != combinedItems[0] || item.craftables[1] != combinedItems[1])
            {
                Debug.Log("Skipped=(Craftable1)" + item);
                continue;
            }

            return item;
        }

        return null;
    }
}
