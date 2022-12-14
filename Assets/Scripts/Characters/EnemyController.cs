using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Enemy Character Controller
/// </summary>
public class EnemyController : CharacterController
{
    [SerializeField] private float attackDelay = 3;

    private void Start() => StartCoroutine(AttackCoroutine());

    private void OnDisable() => StopCoroutine(AttackCoroutine());

    #region Damaged
    public override void RecieveAttack()
    {
        base.RecieveAttack();
        animator.SetBool("Damage", true);
    }
    #endregion

    #region Attack
    public override void Attack()
    {
        animator.SetBool("Attack", true);
        AsyncAttack();
    }

    void AsyncAttack()
    {
        if ((!animator.GetBool("Dead")) && (!animator.GetBool("Damage")) && (animator.GetBool("Attack")))
        {
            base.Attack();
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
