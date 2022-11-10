using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUser : MonoBehaviour
{
    [SerializeField] private StatBar manaBar;

    [SerializeField] private int maxMana = 100;
    public int currentMana { get; private set; }
    [SerializeField]
    [Tooltip("Time span between mana refreshes (per second)")]
    private float regenRate = 0.5f;
    [SerializeField]
    [Tooltip("Amount of mana regenerated over time")]
    private int manaRegenAmount = 1;


    private float nextRegen = 0.0f;


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
}
