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
public class SpellBook : MonoBehaviour
{
    public static SpellBook Instance;

    [Header("References")]
    [SerializeField] private Canvas spellbookUI;
    [SerializeField] private Text level;
    [SerializeField] private Text knowledge;

    // All spells are added on the book on the scene.
    [SerializeField] private GameObject[] spellsGO; // Hold references on memory instead of getting on runtime (?)

<<<<<<< Updated upstream
    public List<SpellSO> discoveredSpells; // to be used for SpellCast
=======
    [Tooltip("Used for Dojo")]
    public List<SpellSO> undiscoveredSpells;
    [Tooltip("Used for battle")]
    public List<SpellSO> discoveredSpells;

    private List<SpellSO> spellList;
    [SerializeField] private bool dojo;

>>>>>>> Stashed changes
    private int knowledgePercentage; // stats (RPG)
    private string levelString;
    //[SerializeField] private CraftManager craftManager;

    private int index = 0; // Current Spell Index

<<<<<<< Updated upstream
    private void Awake()
    {
        //CraftManager.Instance = craftManager;
        //Debug.Log("CraftManager Instance=" + craftManager.name, craftManager.gameObject);
        //Instance = this;
=======
    [Tooltip("Interactive object being animated with spellcast")]
    [SerializeField] ItemSpells item;

    private void Awake()
    {
        if (!dojo)
        {
            spellList = discoveredSpells;
        }
        else
        {
            spellList = undiscoveredSpells;
        }
>>>>>>> Stashed changes
    }

    private void Start()
    {
<<<<<<< Updated upstream
        //craftManager.OnSpellcrafted += AddSpell;
    }

=======
        if (dojo)
            item.OnSpellLearned += AddSpell;
    }


>>>>>>> Stashed changes
    private void OnMouseDown()
    {
        OpenSpellbookUI();
    }

    // Adds the crafted spell to the spellbook.
    // Run from CraftManager on SpellCraft?
    public void AddSpell(SpellSO newSpell)
    {
        discoveredSpells.Add(newSpell);
<<<<<<< Updated upstream
=======
        undiscoveredSpells.Remove(newSpell);
>>>>>>> Stashed changes

        for (int i = 0; i < spellsGO.Length; i++)
        {
            if (newSpell == spellsGO[i].GetComponent<SpellScroll>().spellSO)
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

    public SpellSO GetSpell()
    {
<<<<<<< Updated upstream
        return discoveredSpells[index];
=======
        return spellList[index];
>>>>>>> Stashed changes
    }

    public SpellSO GetNextSpell()
    {
<<<<<<< Updated upstream
        return discoveredSpells[NextInt()];
=======
        return spellList[NextInt()];
>>>>>>> Stashed changes
    }

    public SpellSO GetPreviousSpell()
    {
<<<<<<< Updated upstream
        return discoveredSpells[PrevInt()];
=======
        return spellList[PrevInt()];
>>>>>>> Stashed changes
    }

    private int NextInt()
    {
<<<<<<< Updated upstream
        if (index < (discoveredSpells.Count-1))
=======
        if (index < (spellList.Count-1))
>>>>>>> Stashed changes
        {
            index++;
        }
        else
        {
            index = 0;
        }
        return index;
    }

    private int PrevInt()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
<<<<<<< Updated upstream
            index = (discoveredSpells.Count-1);
=======
            index = (spellList.Count-1);
>>>>>>> Stashed changes
        }
        return index;
    }
}
