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


    [SerializeField] private EnemyStats enemy;

    void Awake()
    {
        foreach (var spell in spellsDiscovered)
        {
            spellBook.discoveredSpells.Add(spell);
        }
    }

    void Start()
    {
        enemy.OnEnemyKilled += EndBattle;
    }


    async void EndBattle()
    {
        await Task.Delay(3000);
        messageText.text = messageString;
        helpText.text = helpString;
        foxCanvas.enabled = true;
    }
}
