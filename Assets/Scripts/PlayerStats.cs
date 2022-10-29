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
    [Tooltip("Time span between mana refreshes (per second)")]
    public float regenRate = 0.5f;
    [Tooltip("Amount of mana regenerated over time")]
    public int manaRegenAmount = 1;
    private float nextRegen = 0.0f;

    [Header("Refactor Reference!")]
    //[SerializeField] private CastManager castManager;
    //[SerializeField] private MagicController magicController;
    [SerializeField] private EnemyStats enemyStats;

    // Todo add refactored spellbook?

    [Tooltip("Time span between mana refreshes (in seconds)")]
    public int manaRegenDelay = 1;
    


    public override void Awake()
    {
        base.Awake();

        currentMana = maxMana;
        if (manaBar)
            manaBar.SetStatMax(maxMana);
    }
    
    public override void Start()
    {
        base.Start();

        // TODO Run from attack  collision?
        enemyStats.OnEnemyAttacked += TakeDamage;
    }

    public void UseMana(int mana)
    {
        currentMana -= mana;
        manaBar.UpdateStat(currentMana);
        Debug.Log(transform.name + " used " + mana + " mana. " + " Remaining mana is " + currentMana);
    }

    private void Update()
    {
        if (currentMana < maxMana && Time.time > nextRegen)
        {
            nextRegen = Time.time + regenRate;
            currentMana += manaRegenAmount;
            manaBar.UpdateStat(currentMana);
        }
    }

    //private IEnumerator ManaRegenCoroutine()
    //{
    //    yield return new WaitUntil(()=>currentMana < maxMana);
    //    //currentMana += manaRegenAmount;
    //    //yield return new WaitForSeconds(manaRegenDelay);
    //}

    //private async void ManaRegen()
    //{
    //    do
    //    {
    //        currentMana += manaRegenAmount;
    //        await System.Threading.Tasks.Task.Delay(manaRegenDelay);
    //    }
    //    while (currentMana < maxMana);
    //}

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
