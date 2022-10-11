using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    public delegate void EnemyAttack(int damage);
    public event EnemyAttack EnemyAttacked;

    public delegate void EnemyKilled();
    public event EnemyKilled OnEnemyKilled;

    //[SerializeField] private CastManager castManager;


    //private void Start()
    //{
    //    castManager.OnSpellCasted += SpellHit;
    //}

    //void SpellHit(SpellSO spell)
    //{
    //    Debug.Log("Spell Hit");
    //    TakeDamage(spell.damage);
    //}

    public void OnEnemyAttacked(int damage)
    {
        EnemyAttacked?.Invoke(damage);
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

    void NextScene()
    {

    }
}
