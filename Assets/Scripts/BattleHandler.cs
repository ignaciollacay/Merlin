using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Counter))]
public class BattleHandler : Singleton<BattleHandler> // TODO: Rename to Manager
{
    private Counter counter;

    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleEnd;

    public override void Awake()
    {
        base.Awake(); // Singleton

        counter = GetComponent<Counter>();

        OnBattleStart?.Invoke();
    }

    public void AddEnemy()
    {
        counter.Increase();
    }

    public void RemoveEnemy()
    {
        counter.Decrease();
    }

    public void StartEndBattleRoutine()
    {
        StartCoroutine(EndBattleRoutine());
    }

    public IEnumerator EndBattleRoutine()
    {
        yield return new WaitForSeconds(2);

        while (counter.BiggerThan(0))
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);

        EndBattle();
    }

    private void EndBattle()
    {
        OnBattleEnd?.Invoke();
    }

    public void SetEnemies(EnemySOList enemies)
    {
        foreach (var enemy in enemies.enemyList)
        {
            Enemy newEnemy = Enemy.CreateEnemy(enemy);
            newEnemy.stats.OnEnemySpawn.AddListener(AddEnemy); // Llegará a agregarse a tiempo? O ya fue instanceado en la linea anterior y ejecutó su awake?
                                                               // A esta altura igual no necesito manejarme con este evento al tener la referencia dentro de la clase.
            newEnemy.stats.OnEnemyKilled.AddListener(RemoveEnemy);

        }
    }
}