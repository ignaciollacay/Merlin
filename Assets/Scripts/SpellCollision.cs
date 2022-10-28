using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be added when instantiating the object?
// or added to the prefabs?

public class SpellCollision : MonoBehaviour
{
    // Podría settearlo OnValidate. Total ya sé cual es el spellSo correspondiente al prefab.
    // Me parmite eludir el problema en MagicController de si el script está en el padre o hijo
    // Solo sirve en el caso de que quiera tener modificadores.
    //[Tooltip ("Defined on instantiation")]
    public SpellSO spell;
    //public int spellDamage;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStats>().TakeDamage(spell.damage);
            //other.GetComponent<EnemyStats>().TakeDamage(spellDamage);
        }
    }
}
