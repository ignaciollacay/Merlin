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
    [SerializeField] private Canvas foxCanvas;
    [SerializeField] private Text messageText;
    [SerializeField] private Text helpText;
    [SerializeField] private string messageString = "Bien has derrotado al enemigo y salvado al planeta";
    [SerializeField] private string helpString = "Presiona para salir del combate";


    [SerializeField] private List<EnemyStats> enemies;
    private int enemyCount;

    void Awake()
    {
        foreach (var spell in spellsDiscovered)
        {
            spellBook.discoveredSpells.Add(spell);
        }
    }

    void Start()
    {
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
            await Task.Delay(3000);
            messageText.text = messageString;
            helpText.text = helpString;
            foxCanvas.enabled = true;
        }
    }
    private bool RemainingEnemies()
    {
        enemyCount--;

        if (enemyCount == 0)
        {
            Debug.Log("All enemies have been defeated");
            return true;
        }
        else
        {
            Debug.Log("One or more enemies are remaining alive");
            return false;
        }
    }




}
