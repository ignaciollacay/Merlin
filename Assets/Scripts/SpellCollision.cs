using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be added when instantiating the object?
// or added to the prefabs?

public class SpellCollision : MonoBehaviour
{
    public SpellSO spell;

    //public event EventHandler OnSpellHit;
 

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStats>().TakeDamage(spell.damage);
            other.GetComponent<EnemyController>().RecieveAttack();

            //OnSpellHit?.Invoke(this, EventArgs.Empty);
        }
    }
}
