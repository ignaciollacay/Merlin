using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    // TODO Remove EnemyAttacked event.
    // Should by run on collision with the vfx.
    // Avoids using event references
    // Avoids asynchronity between attack function & visual.
    public delegate void EnemyAttacked(int damage);
    public event EnemyAttacked OnEnemyAttacked;

    public delegate void EnemyKilled();
    public event EnemyKilled OnEnemyKilled;

    //[SerializeField] private CastManager castManager;
    [Header("Refactor Reference!")]
    public MagicController magicController;

    // TODO Remove. See note above.
    public void EnemyAttack(int damage)
    {
        Debug.Log("Enemy Attacked Event");
        OnEnemyAttacked.Invoke(damage);
    }

    public override void Death()
    {
        base.Death();
        Destroy(gameObject);
        OnEnemyKilled?.Invoke();
    }

    void DeathAnim()
    {
        // Play Death Anim

        // Destroy GO

        // Trigger p
    }
}
