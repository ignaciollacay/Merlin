using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    public SpellBook spellBook;
    public List<SpellSO> spellsTutorial;
    public List<SpellSO> spellsFight;

    [Header("UI")]
    [SerializeField] private Pet pet;



    void Awake()
    {
        AddSpells(spellsTutorial);
        pet.TutorialIntro();
        StartCoroutine(FightingSpells());
    }

    IEnumerator FightingSpells()
    {
        yield return new WaitUntil(()=> spellBook.discoveredSpells.Count == 3);
        AddSpells(spellsFight);
        pet.FirstSpellsIntro();
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        yield return new WaitUntil(() => spellBook.discoveredSpells.Count == 6);
        pet.BattleStart();
        yield return new WaitUntil(() => dialogueManager.dialogueEnd);
        SceneManager.LoadScene(1);
    }



    void AddSpells(List<SpellSO> spells)
    {
        foreach (var spell in spells)
        {
            spellBook.undiscoveredSpells.Add(spell);
        }
    }
}
