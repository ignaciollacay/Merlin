using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TestingCastBattleManager : MonoBehaviour
{
    public SpellBook spellBook;
    public List<SpellSO> spellsDiscovered;


    [Header("Refs")]
    [SerializeField] Pet pet;
    [SerializeField] private Canvas input;
    [SerializeField] Canvas exitButton;

    [SerializeField] private List<EnemyStats> enemies;
    private int enemyCount;

    void Awake()
    {
        foreach (var spell in spellsDiscovered)
        {
            spellBook.discoveredSpells.spells.Add(spell);
        }
    }
    async void NextSpell()
    {
        await Task.Delay(1000);
        FindObjectOfType<CastManager>().NextSpell();
    }

    void Start()
    {
        FindObjectOfType<PhraseRecognition>().OnPhraseRecognized += NextSpell;

        enemyCount = enemies.Count;

        foreach (var enemy in enemies)
        {
            enemy.OnEnemyKilled += EndBattle;
        }
    }

    async void EndBattle()
    {
        if (!RemainingEnemies())
        {
            await Task.Delay(6000);
            input.enabled = false;
            pet.BattleEnd();
            await Task.Delay(6000);
            exitButton.enabled = true;
        }
    }
    private bool RemainingEnemies()
    {
        enemyCount--;

        if (enemyCount == 0)
        {
            Debug.Log("All enemies have been defeated");
            return false;
        }
        else
        {
            Debug.Log("One or more enemies are remaining alive");
            return true;
        }
    }




}
