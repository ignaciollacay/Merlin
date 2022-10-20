using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [Header("Char References")]
    [SerializeField] private StatBar healthBar;

    [Header("Char Values")]
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public int defense = 0;


    public virtual void Awake()
    {
        currentHealth = maxHealth;
        if (healthBar)
            healthBar.SetStatMax(maxHealth);
        //Debug.Log(transform.name + " current life is " + currentHealth);
    }


    public void TakeDamage (int _damage)
    {
        int damage = CalculateDefense(_damage);

        currentHealth -= damage;
        healthBar.UpdateStat(currentHealth);
        Debug.Log(transform.name + " attacked for " + damage + " damage. " + " Remaining life is " + currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Defend(int value)
    {

    }

    // TODO Defense mechanic needs to be designed & Updated.
    private int CalculateDefense(int damage)
    {
        int finalDamage = damage - defense;
        Debug.Log("Damage after defense=" + finalDamage);
        if (finalDamage > 0)
        {
            return finalDamage;
        }
        else
        {
            return 0;
        }

    }

    public virtual void Death()
    {
        Debug.Log(transform.name + " died.");
    }
}
