using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be added when instantiating the object?
// or added to the prefabs?


public class SpellCollision : MonoBehaviour
{
    public SpellSO spell;

    
    private void OnParticleCollision(GameObject other)
    {
        TakeDamage(other);
    }

    // TODO: Should send an event
    // and subscribe from the corresponding stats or controller.
    private void TakeDamage(GameObject other)
    {
        if (other.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(spell.damage);
        }
        else if (other.TryGetComponent(out PlayerStats player))
        {
            player.TakeDamage(spell.damage);
        }
    }
}
