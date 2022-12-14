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

    [Header("Advanced Attack Propierties")]
    [SerializeField] private bool NonCollisionAttacker = false;
    [SerializeField] private int damageDelayInMiliseconds = 500;

    private PlayerStats playerStats; // FIXME: Dirty dependency

    public override void Awake()
    {
        base.Awake();

        if (NonCollisionAttacker)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
    }
    public override void Start()
    {
        base.Start();
        OnEnemySpawn?.Invoke();
    }
    // TODO: Es medio raro que la llamo desde el evento del controller. Pero necesitaba tomar el valor de da?o de los stats y no lo queria referenciar
    public void AttackEvent()
    {
        Debug.Log("Enemy Attacked Event" + name, gameObject);
        RemoveHealthFromOpponent();
        OnEnemyAttack?.Invoke(attackDamage);
    }

    public override void Death()
    {
        base.Death();
        if (NonCollisionAttacker)
        {
            OnEnemyKilled?.Invoke();
        }
        Debug.Log("OnEnemyKilled.Invoked" + name, gameObject);
    }

    private async void RemoveHealthFromOpponent()
    {
        if (NonCollisionAttacker)
        {
            await System.Threading.Tasks.Task.Delay(damageDelayInMiliseconds);
            playerStats.TakeDamage(attackDamage);
        }
    }
}
