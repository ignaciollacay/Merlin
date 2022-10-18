using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //desprolijo, sacar de este script -- TBD

public class PlayerStats : CharacterStats
{
    [Header("Player References")]
    [SerializeField] private StatBar manaBar;

    [Header("Player Values")]
    public int life = 100;
    public int maxMana = 100;
    public int currentMana { get; private set; } //lo usa como int, idem playerHealth

    [Header("Refactor Reference!")]
    //[SerializeField] private CastManager castManager;
    //[SerializeField] private MagicController magicController;
    [SerializeField] private EnemyStats enemyStats;

    // Todo add refactored spellbook?

    public override void Awake()
    {
        base.Awake();

        currentMana = maxMana;
        manaBar.SetStatMax(maxMana);
        Debug.Log(transform.name + " current mana is " + currentMana);
    }
    
    private void Start()
    {
        StartCoroutine(DeathCoroutine());

        // TODO Run from attack  collision?
        enemyStats.OnEnemyAttacked += TakeDamage;

        // Obsolete. Run directly from magic controller
        //magicController.OnSpellFired += UseMana;
    }

    // Obsolete. Event replaced.
    //public void UseMana(SpellSO spell)
    //{
    //    UseMana(spell.mana);
    //}

    public void UseMana(int mana)
    {
        currentMana -= mana;
        manaBar.UpdateStat(currentMana);
        Debug.Log(transform.name + " used " + mana + " mana. " + " Remaining mana is " + currentMana);
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitUntil(() => life <= 0);
        Death();
    }

    public override void Death()
    {
        base.Death();

        ResetScene();
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
