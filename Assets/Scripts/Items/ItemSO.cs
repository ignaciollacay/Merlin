using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Basic,
    Crafted
}

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Item Properties")]
    public string Name;
    public GameObject prefab;
    public ItemType type = ItemType.Basic;
    public ItemSO[] craftables = new ItemSO[2];

    private void OnValidate()
    {
        if (craftables[0] == null && craftables[1] == null)
        {
            return;
        }
        else if (craftables[0] == null || craftables[1] == null)
        {
            Debug.LogWarning("Missing craftable. Link two items");
        }
        else
        {
            type = ItemType.Crafted;
        }
    }
}
