using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100; //lo usa como int, para que no tenga modifiers; quisiera que sea un stat pero complicado para el damage?
    public int currentHealth { get; private set; } //lo usa como int, idem playerHealth

    [SerializeField] private HealthBar healthBar;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Debug.Log(transform.name + " current life is " + currentHealth);
    }

    public void TakeDamage (int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealth(currentHealth);
        Debug.Log(transform.name + " attacked for " + damage + " damage. " + " Remaining enemy life is " + currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        Debug.Log(transform.name + " died.");
    }
}
