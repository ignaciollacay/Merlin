using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Level
{
    Low,
    Mid,
    High,
    Complete
}

// TODO ScriptableObject? Works like inventory
//[DefaultExecutionOrder(-3)]
[RequireComponent(typeof(Counter))]
public class SpellBook : MonoBehaviour
{
    public static SpellBook Instance;

    [Header("References")]
    [SerializeField] private Canvas spellbookUI;
    [SerializeField] private Text level;
    [SerializeField] private Text knowledge;
    [SerializeField] private AudioSource spellDiscoveredSFX;

    // All spells are added on the book on the scene.
    [SerializeField] private GameObject[] spellsGO; // Hold references on memory instead of getting on runtime (?)

    [Tooltip("Used for Dojo")]
    public List<SpellSO> undiscoveredSpells;
    [Tooltip("Used for battle")]
    public List<SpellSO> discoveredSpells;

    private List<SpellSO> spellList;
    [SerializeField] private bool dojo;

    private int knowledgePercentage; // stats (RPG)
    private string levelString;
    //[SerializeField] private CraftManager craftManager;

    // TODO: Replace with Counter
    //private int index = 0; // Current Spell Index
    private Counter counter;

    [Tooltip("Interactive object being animated with spellcast")]
    [SerializeField] ItemSpells item;

    private void Awake()
    {
        counter = GetComponent<Counter>();

        if (!dojo)
        {
            spellList = discoveredSpells;
        }
        else
        {
            spellList = undiscoveredSpells;
        }
    }

    private void Start()
    {
        if (dojo)
            item.OnSpellLearned += AddSpell;
    }


    private void OnMouseDown()
    {
        OpenSpellbookUI();
    }

    // Adds the crafted spell to the spellbook.
    // Run from CraftManager on SpellCraft?
    public void AddSpell(SpellSO newSpell)
    {
        if (isDiscovered(newSpell))
        {
            print("spell is already discovered. Returning, shouldn't be added");
            return;
        }

        discoveredSpells.Add(newSpell);
        undiscoveredSpells.Remove(newSpell);
        spellDiscoveredSFX.Play();

        for (int i = 0; i < spellsGO.Length; i++)
        {
            if (newSpell == spellsGO[i].GetComponent<SpellScroll>().spellSO)
            {
                spellsGO[i].SetActive(true);
            }
        }

        UpdateKnowledge();

        FindObjectOfType<CastManager>().NextSpell();
    }

    private bool isDiscovered(SpellSO newSpell)
    {
        foreach (var spell in discoveredSpells)
        {
            if (spell == newSpell)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateKnowledge()
    {
        // Get current knowledge value
        knowledgePercentage = discoveredSpells.Count * 100 / spellsGO.Length;

        // Get current level
        GetLevel(knowledgePercentage);

        // Update UI
        level.text = "Nivel: " + levelString;
        knowledge.text = "Conocimiento: " + discoveredSpells.Count + "/" + spellsGO.Length;
    }

    // FIXME
    public Level GetLevel(int value)
    {
        if (value < 33)
        {
            levelString = "Aprendiz";
            return Level.Low;
        }
        else if (value < 66)
        {
            levelString = "Intermedio";
            return Level.Mid;
        }
        else if (value < 100)
        {
            levelString = "Avanzado";
            return Level.High;
        }
        else
        {
            levelString = "Maestro";
            return Level.Complete;
        }
    }

    // Accessed by BookPile EventTrigger OnPointerClick
    public void OpenSpellbookUI()
    {
        spellbookUI.enabled = true;
    }

    // Accessed by Spellbook Canvas
    public void CloseSpellbookUI()
    {
        spellbookUI.enabled = false;
    }

    public SpellSO GetSpell()
    {
        return spellList[counter.count];
    }

    public SpellSO GetNextSpell()
    {
        return spellList[counter.Next(spellList.Count)];
    }

    public SpellSO GetPreviousSpell()
    {
        return spellList[counter.Next(spellList.Count)];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Counter=" + counter.count + "\n Spell=" + spellList[counter.count]);
            GetNextSpell();
            Debug.Log("Counter=" + counter.count + "\n Spell=" + spellList[counter.count]);
        }
    }
}
