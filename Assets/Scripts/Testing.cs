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
        //AddSpells(spellsTutorial);
        //pet.TutorialIntro();
        //StartCoroutine(FightingSpells());
    }

    IEnumerator FightingSpells() // TODO: Make reutilizable for different cases with inspector and/or parameters
    {
        yield return new WaitUntil(()=> spellBook.discoveredSpells.GetCount() == 3);
        AddSpells(spellsFight);
        FindObjectOfType<CastManager>().NextSpell();
        pet.FirstSpellsIntro();
        StopCoroutine(FightingSpells());
        //StartCoroutine(Battle());
    }

    /* Replaced by BattleStarter
    IEnumerator Battle() // FIXME: WIP To remove this. 
    {
        StopCoroutine(Battle());
        yield return new WaitUntil(() => spellBook.discoveredSpells.GetCount() == 5);
        pet.FirstBattle();
        yield return new WaitUntil(() => DialogueManager.Instance.dialogueEnd);
        SceneManager.LoadScene(1);
    }
    */
    void AddSpells(List<SpellSO> spells)
    {
        foreach (var spell in spells)
        {
            spellBook.undiscoveredSpells.spells.Add(spell);
        }
    }
}