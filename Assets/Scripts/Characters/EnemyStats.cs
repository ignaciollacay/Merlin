using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : CharacterStats
{
    public override CharacterType CharacterType { get; set; } = CharacterType.Enemy;

    public UnityEvent OnEnemySpawn;
    public UnityEvent OnEnemyKilled;
    public UnityEvent<int> OnEnemyAttack;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        OnEnemySpawn?.Invoke();
    }
    // TODO: Es medio raro que la llamo desde el evento del controller. Pero necesitaba tomar el valor de daño de los stats y no lo queria referenciar
    public void AttackEvent()
    {
        Debug.Log("Enemy Attacked Event" + name, gameObject);
        OnEnemyAttack?.Invoke(attackDamage);
    }

    public override void Death()
    {
        base.Death();
        OnEnemyKilled?.Invoke();
        Debug.Log("OnEnemyKilled.Invoked" + name, gameObject);
    }
}
