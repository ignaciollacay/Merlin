using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : CharacterStats
{
    public UnityEvent OnEnemySpawn;
    public UnityEvent OnEnemyKilled;
    public UnityEvent<int> OnEnemyAttack;

    public void SubtractHealth()
    {
        Debug.Log("Enemy Attacked Event");
        OnEnemyAttack?.Invoke(attackDamage);
    }

    public override void Death()
    {
        base.Death();
        OnEnemyKilled?.Invoke();
        Debug.Log("OnEnemyKilled.Invoked" + this.name, this.gameObject);
    }
}
