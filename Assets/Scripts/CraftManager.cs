using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

public class CraftManager : MonoBehaviour
{
    //singleton?
    public static CraftManager Instance { get; set; }

    [SerializeField] private ItemDatabaseSO itemDatabaseSO;
    [SerializeField] private Transform SpawnIngredient;
    [SerializeField] private Transform SpawnCrafted;
    [SerializeField] private ParticleSystem craftVFX;


    private ItemSO[] itemMixSO = new ItemSO[2];
    private GameObject[] itemMixGO = new GameObject[2];


    [Header("Advanced Settings")]
    [Tooltip("Time delay for new crafted item after both ingredients are selected by speech")]
    [Range(0, 4000)]
    [SerializeField] private int craftDelay = 1500;
    [Tooltip("Value applied to min/max values of the range for Random XY position around the center of the pot")]
    [Range(0, 1)]
    [SerializeField] private float randomSpawnPos = 0.25f;


    //public delegate void ItemCrafted();
    //public event ItemCrafted OnItemCrafted;


    private void Awake()
    {
        Instance = this;
    }


    Vector3 GetRandomPosition()
    {
        float min = -randomSpawnPos;
        float max = randomSpawnPos;
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);

        Vector3 randomPos = new Vector3(x, y, 0);
        Vector3 newPos = SpawnIngredient.position + randomPos;

        return newPos;
    }

    public void ItemsSelected(ItemSO item)
    {
        if (itemMixSO[0] == null)
        {
            //Spawn item
            itemMixGO[0] = Instantiate(item.prefab, GetRandomPosition(), SpawnIngredient.rotation, SpawnIngredient);

            //Add to combined
            itemMixSO[0] = item;

            Debug.Log("First item to craft selected.");
        }
        else if (itemMixSO[1] == null)
        {
            //Spawn item
            itemMixGO[1] = Instantiate(item.prefab, GetRandomPosition(), SpawnIngredient.rotation, SpawnIngredient);

            //Add to combined
            itemMixSO[1] = item;

            Debug.Log("Both items were combined. Crafting new item!");

            CraftItem();
        }
        else
        {
            //Both items were combined. Craft new item
            Debug.LogWarning("Unexpected result");
        }
    }

    private async void CraftItem()
    {
        ItemSO item = GetItem();

        if (item != null)
        {
            await Task.Delay(1500);

            //Event
            //OnItemCrafted.Invoke();

            // Destroy spawned ingredients
            foreach (var spawnedItem in itemMixGO)
                Destroy(spawnedItem);

            // Create crafted item
            Instantiate(item.prefab, SpawnCrafted);

            // VFX
            craftVFX.Play();

            Debug.Log("Crafted=" + item);
        }
        else
        {
            Debug.Log("Crafted item could not be found. Check ItemSO.");
        }
    }

    private ItemSO GetItem()
    {
        ItemSO get = null;

        foreach (ItemSO item in itemDatabaseSO.Items)
        {
            if (item.type != ItemType.Crafted)
                continue;

            foreach (ItemSO craftable in item.craftables)
            {
                if (craftable == itemMixSO[0])
                {
                    get = item;
                }
                else if (craftable == itemMixSO[1])
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

        return get;
    }
}