using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

//[DefaultExecutionOrder(-2)]
public class CraftManager : MonoBehaviour
{
    //singleton?
    public static CraftManager Instance { get; set; }

    [Header("References")]
    public ItemDatabaseSO itemDatabaseSO;
    public SpellCraft spellDisplay;
    [SerializeField] private AzureSpeechRecognition speechManager;
    [SerializeField] private SpellBook spellBook;

    [Header("Spawn Height")]
    [SerializeField] float spawnIngredient = 1.167f;
    [SerializeField] float spawnCrafted = 0.339f;

    [Header("Event References")]
    //[SerializeField] private Transform SpawnIngredient;
    //[SerializeField] private Transform SpawnCrafted;
    [SerializeField] private ParticleSystem craftVFX;
    [SerializeField] private Canvas foxCanvas;
    [SerializeField] private Text foxText;
    [SerializeField] private GameObject ingredient1;
    [SerializeField] private GameObject ingredient2;
    [SerializeField] private GameObject ingredient3;
    [SerializeField] private GameObject ingredient4;

    [Header("Advanced Settings")]
    //[Tooltip("Time delay for new crafted item after both ingredients are selected by speech")]
    //[Range(0, 4000)]
    //[SerializeField] private int craftDelay = 1500;
    [Tooltip("Value applied to min/max values of the range for Random XY position around the center of the pot")]
    [Range(0, 1)]
    [SerializeField] private float randomSpawnPos = 0.25f;

    private ItemSO[] itemMixSO = new ItemSO[2];
    private GameObject[] itemMixGO = new GameObject[2]; // hold Spawned GameObject Reference to Destroy after use
    private GameObject itemCrafted;                     // hold Spawned GameObject Reference to Destroy after use



    // Event
    //public delegate void Spellcrafted();
    //public event Spellcrafted OnSpellcrafted;


    private void Awake()
    {
        //speechManager = SpeechRecognition.Instance;
        //Debug.Log("Speech Instance=" + speechManager.name, speechManager.gameObject);
        //Instance = this;
        //spellBook = SpellBook.Instance;
        //Debug.Log("Spellbook Instance=" + spellBook.name, spellBook.gameObject);
    }

    public void ItemsSelected(ItemSO item)
    {
        if (itemMixSO[0] == null)
        {
            //Spawn item
            itemMixGO[0] = Instantiate(item.prefab, GetRandomPosition(), transform.rotation);

            //Add to mix
            itemMixSO[0] = item;

            Debug.Log("ItemMix1=" + item);
        }
        else if (itemMixSO[1] == null)
        {
            //Spawn item
            itemMixGO[1] = Instantiate(item.prefab, GetRandomPosition(), transform.rotation);

            //Add to mix
            itemMixSO[1] = item;

            Debug.Log("ItemMix2=" + item);

            speechManager.StopSpeechRecognition();

            DisplaySpell(); //CraftItem();
        }
        else
        {
            Debug.LogWarning("Unexpected result");
        }
    }

    // A new spell has been discovered.
    // Display spell & add to spellbook
    private void DisplaySpell()
    {
        SpellSO spell = itemDatabaseSO.GetSpell(itemMixSO[0], itemMixSO[1]);

        if (spell != null)
        {
            // Display Spell in UI
            spellDisplay.spellSO = spell;
            spellDisplay.SetSpell();

            // Add the spell on spellbook
            spellBook.AddSpell(spell);
        }
        else
        {
            Debug.Log("A Spell matching selected ingredients could not be found.");
        }
    }

    // Executed after correct Spell Cast from Spell script in spellbook.
    public void CraftSpell(ItemSO item)
    {
        //Event
        //OnSpellcrafted.Invoke();
        OnSpellCraft(item);
        
    }

    private async void OnSpellCraft(ItemSO item)
    {
        // VFX
        craftVFX.Play();

        // Destroy spawned ingredients
        foreach (var spawnedItem in itemMixGO)
            Destroy(spawnedItem);

        // Create crafted item
        itemCrafted = Instantiate(item.prefab, transform.position + new Vector3(0,spawnCrafted,0), transform.rotation);

        await Task.Delay(2000);

        // UI Pop Up
        string message = "Has desbloqueado un nuevo ingrediente y hechizo!";
        foxText.text = message;
        foxCanvas.enabled = true;

        // Reset mix
        itemMixSO = new ItemSO[2];
        itemMixGO = new GameObject[2];

        // Reset Spell (& Phrase Recognition)
        spellDisplay.ResetSpell();

        await Task.Delay(2000);

        // Switch ingredients
        ingredient1.SetActive(false);
        ingredient2.SetActive(false);
        ingredient3.SetActive(true);
        ingredient4.SetActive(true);

        // Remove crafted ingredient.
        Destroy(itemCrafted);
    }

    Vector3 GetRandomPosition()
    {
        float min = -randomSpawnPos;
        float max = randomSpawnPos;
        float x = Random.Range(min, max);
        float z = Random.Range(min, max);

        Vector3 randomPos = new Vector3(x, spawnIngredient, z);
        Vector3 newPos = transform.position + randomPos;

        return newPos;
    }
}

// Backup version
// Executed after selected ingredients.
/* public void CraftItem()
{
    ItemSO item = GetItem();

    if (item != null)
    {
        //Event
        //OnItemCrafted.Invoke();

        // Destroy spawned ingredients
        foreach (var spawnedItem in itemMixGO)
            Destroy(spawnedItem);

        // Create crafted item
        Instantiate(item.prefab, SpawnCrafted);

        // VFX
        craftVFX.Play();
    }
    else
    {
        Debug.Log("Crafted item could not be found. Check ItemSO.");
    }
}*/