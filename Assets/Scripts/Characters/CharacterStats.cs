using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class CharacterStats : MonoBehaviour
{
    public virtual CharacterType CharacterType { get; set; }

    public UnityEvent<int> OnCharacterSpawn;
    public UnityEvent<int> OnHealthUpdate;

    [Header("Char Values")]
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public int attackDamage;
    public int defense = 0;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        OnCharacterSpawn?.Invoke(maxHealth);
    }
    public virtual void Start()
    {
        StartCoroutine(DeathCoroutine());
    }

    /// <summary>
    /// Recieve damage. Updates Stats, UI and checks if object should die if life is lower than zero
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage)
    {
        int damage = CalculateDefense(_damage);
        currentHealth -= damage;
        OnHealthUpdate?.Invoke(currentHealth);
        Debug.Log(transform.name + " took damage=" + damage + ". Life remaining is " + currentHealth + " / " + name, gameObject);
    }

    // TODO Defense mechanic needs to be designed & Updated.
    private int CalculateDefense(int damage)
    {
        int finalDamage = damage - defense;

        if (finalDamage > 0)
            return finalDamage;
        else
            return 0;
    }

    public virtual void Death()
    {
        Debug.Log(transform.name + " died.");
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitUntil(()=> currentHealth <= 0);
        Death();
    }

    // TODO: Not Yet Implemented. (On Player) Update damage stat when spell is chosen, spell button click or OnSpellHit
    private void UpdateAttackDamage(int newAttackDamage)
    {
        attackDamage = newAttackDamage;
    }
}
