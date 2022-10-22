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

    [SerializeField] private AudioSource spellSFX;

    void Awake()
    {
        FindObjectOfType<CastManager>().OnSpellCasted += magicSFX;
        AddSpells(spellsTutorial);
        pet.TutorialIntro();
        StartCoroutine(FightingSpells());
    }
    void magicSFX(SpellSO spell)
    {
        spellSFX.Play();
    }
    IEnumerator FightingSpells()
    {
        yield return new WaitUntil(()=> spellBook.discoveredSpells.Count == 2);
        AddSpells(spellsFight);
        FindObjectOfType<CastManager>().NextSpell();
        pet.FirstSpellsIntro();
        StopCoroutine(FightingSpells());
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        StopCoroutine(Battle());
        yield return new WaitUntil(() => spellBook.discoveredSpells.Count == 4);
        pet.FirstBattle();
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
