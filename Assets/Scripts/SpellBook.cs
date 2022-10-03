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


public class SpellBook : MonoBehaviour
{
    public static SpellBook Instance;

    [Header("References")]
    [SerializeField] private Canvas spellbookUI;
    [SerializeField] private Text level;
    [SerializeField] private Text knowledge;

    // All spells are added on the book on the scene.
    [SerializeField] private GameObject[] spellsGO; // Hold references on memory instead of getting on runtime (?)

    private List<SpellSO> discoveredSpells; // to be used for SpellCast
    private int knowledgePercentage; // stats (RPG)
    private string levelString;
    private CraftManager craftManager;

    private void Awake()
    {
        Instance = this;

        CraftManager.Instance = craftManager;
    }

    private void Start()
    {
        //craftManager.OnSpellcrafted += AddSpell;
    }


    // Adds the crafted spell to the spellbook.
    // Run from CraftManager on SpellCraft?
    public void AddSpell(SpellSO newSpell)
    {
        discoveredSpells.Add(newSpell);

        for (int i = 0; i < spellsGO.Length; i++)
        {
            if (newSpell == spellsGO[i].GetComponent<SpellSO>())
            {
                spellsGO[i].SetActive(true);
            }
        }

        UpdateKnowledge();
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
}
