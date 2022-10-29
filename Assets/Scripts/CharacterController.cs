using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private EnemyStats stats;
    public PlayerStats playerstats;
    private Animator _animator;

    [SerializeField] private ParticleSystem vfxAttack;
    [SerializeField] private ParticleSystem vfxAttacked;

    public float attackDelay;
    public float vfxDelay = 0.5f;
    public float dmgDelay = 1;


    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        StartCoroutine(AttackCoroutine());
        stats.OnEnemyKilled += Death;
        //stats.OnEnemyAttacked += TakeDamage;
    }

    private void OnDisable()
    {
        StopCoroutine(AttackCoroutine());
        stats.OnEnemyKilled -= Death;
        //stats.OnEnemyAttacked -= TakeDamage;
    }

    public async void Death()
    {
        _animator.SetBool("Dead", true);
        await System.Threading.Tasks.Task.Delay(4000);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        _animator.SetBool("Damage", true);
        vfxAttacked.Play();
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        Attack();
        StartCoroutine(AttackCoroutine());
    }

    async void Attack()
    {
        _animator.SetBool("Attack", true);  
        await System.Threading.Tasks.Task.Delay(((int)(vfxDelay * 1000)));
        vfxAttack.Play();
        playerstats.TakeDamage(stats.damage);
    }
}
