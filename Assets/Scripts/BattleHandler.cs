using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Counter))]
public class BattleHandler : MonoBehaviour
{
    private Counter counter;

    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleEnd;

    void Awake()
    {
        counter = GetComponent<Counter>();

        foreach (EnemyStats enemy in FindEnemyCount())
        {
            enemy.OnEnemyAttack.AddListener(FindObjectOfType<PlayerStats>().TakeDamage);
            enemy.OnEnemyKilled.AddListener(EndBattle);
        }

        OnBattleStart?.Invoke();
    }


    // Find enemies, set enemy count & return as list.
    private EnemyStats[] FindEnemyCount()
    {
        EnemyStats[] enemies = FindObjectsOfType<EnemyStats>();
        counter.Count = enemies.Length;
        Debug.Log("EnemyCount=" + counter.Count);
        return enemies;
    }

    private async void EndBattle()
    {
        if (!RemainingEnemies())
        {
            await Task.Delay(6000); // Wait for Death Animation to finish. 
                                    // TODO: Should Death Anim invoke OnEnemyKilled instead of health stat?
            Debug.Log("EnemyCount=" + counter.Count);
            Debug.Log("All enemies have been defeated");
            OnBattleEnd?.Invoke();
            SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
            DialogueManager.Instance.DialogueEnd.AddListener(sceneHandler.LoadAsync);
        }
    }

    private bool RemainingEnemies()
    {
        if (counter.Decrease() == 0)
            return false;
        else
            return true;
    }

    // TODO: Can replace find enemy count by Calling AddEnemy from OnEnemySpawn Unity Event.
    public void AddEnemy()
    {
        counter.Increase();
    }
}