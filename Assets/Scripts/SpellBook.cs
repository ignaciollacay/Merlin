using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(Counter))]
public class SpellBook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas spellbookUI;
    [SerializeField] private Text level;
    [SerializeField] private Text knowledge;
    [SerializeField] private AudioSource spellDiscoveredSFX;

    // All spells are added on the book on the scene.
    [SerializeField] private GameObject[] spellsGO; // Hold references on memory instead of getting on runtime (?)

    [Tooltip("Used for Dojo")]
    public InventorySpellSO undiscoveredSpells;
    [Tooltip("Used for battle")]
    public InventorySpellSO discoveredSpells;

    [SerializeField] private bool dojo;

    private Counter counter;

    public UnityEvent OnSpellLearned; // TODO Use only OnSpellCasted?

    private void Awake()
    {
        counter = GetComponent<Counter>();
    }

    // Adds the crafted spell to the spellbook.
    // Run from CraftManager on SpellCraft?
    public void AddSpell(SpellSO newSpell)
    {
        OnSpellLearned?.Invoke();

        discoveredSpells.spells.Add(newSpell);
        spellDiscoveredSFX.Play();

        for (int i = 0; i < spellsGO.Length; i++)
        {
            if (newSpell == spellsGO[i].GetComponent<SpellScroll>().spellSO)
            {
                spellsGO[i].SetActive(true);
            }
        }

        FindObjectOfType<CastManager>().NextSpell(); // FIXME: Remove Find (expensive & dependancy)
    }

    #region // TODO: Leveling system
    // TODO: Leveling system
    private int knowledgePercentage; // stats (RPG)
    private string levelString;
    public enum Level
    {
        Low,
        Mid,
        High,
        Complete
    }
    private void UpdateKnowledge()
    {
        // Get current knowledge value
        knowledgePercentage = discoveredSpells.GetCount() * 100 / spellsGO.Length;

        // Get current level
        GetLevel(knowledgePercentage);

        // Update UI
        level.text = "Nivel: " + levelString;
        knowledge.text = "Conocimiento: " + discoveredSpells.GetCount() + "/" + spellsGO.Length;
    }
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
    #endregion
}
