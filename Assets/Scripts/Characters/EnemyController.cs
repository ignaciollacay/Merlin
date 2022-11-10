using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Enemy Character Controller
/// </summary>
public class EnemyController : CharacterController
{
    [SerializeField] private ParticleSystem vfxAttack;
    [SerializeField] private ParticleSystem vfxAttacked;

    public float attackDelay;
    public float vfxDelay = 0.5f;
    public float dmgDelay = 1;

    private void Start() => StartCoroutine(AttackCoroutine());

    private void OnDisable() => StopCoroutine(AttackCoroutine());

    #region Damaged
    public override void RecieveAttack()
    {
        base.RecieveAttack();
        animator.SetBool("Damage", true);
        vfxAttacked.Play();
    }
    #endregion

    #region Attack
    public override void Attack()
    {
        animator.SetBool("Attack", true);
        AsyncAttack();
    }

    async void AsyncAttack()
    {
        await System.Threading.Tasks.Task.Delay(((int)(vfxDelay * 1000)));
        if ((!animator.GetBool("Dead")) && (!animator.GetBool("Damage")) && (animator.GetBool("Attack")))
        {
            vfxAttack.Play();
            base.Attack();
            //playerstats.TakeDamage(stats.damage);
        }
            
        StartCoroutine(AttackCoroutine());
    }
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        Attack();
    }
    #endregion

    #region Death
    public override void Death()
    {
        animator.SetBool("Dead", true);
        StartCoroutine(DestroyOnAnimationEnd());
    }

    IEnumerator DestroyOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    #endregion
}
