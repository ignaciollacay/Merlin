using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be added when instantiating the object?
// or added to the prefabs?

public enum CharacterType
{
    Enemy,
    Player
}


// TODO: Decouple Collision from Spell Collision
// Move everything except Particle Collision to parent AttackCollision Class
// and inherit from it in SpellCollision, using only OnParticleCollision.
// Allowing to reutilize the code cleanly in MeleeCollision
// Rename class to RangedParticleCollision?
public class SpellCollision : MonoBehaviour
{
    public SpellSO spell;
    private CharacterType m_characterType;

    private void Awake()
    {
        GetCharacterTypeFromParent();
    }
    
    private void OnParticleCollision(GameObject other)
    {
        SpellCollided(other);
    }

    // TODO: Should send an event, to reuse ParticleCollision. Could subscribe from the corresponding stats or controller.
    public void SpellCollided(GameObject other)
    {
        CharacterStats otherStats = GetStatsFromOther(other);
        if (otherStats == null)
        {
            return;
        }
        if (IsOpponent(otherStats))
        {
            otherStats.TakeDamage(spell.damage);
        }
    }
    CharacterStats GetStatsFromOther(GameObject other)
    {
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            return playerStats;
        }
        else if (other.TryGetComponent(out EnemyStats enemyStats))
        {
            return enemyStats;
        }
        else
        {
            return null;
        }
    }
    bool IsOpponent(CharacterStats stats)
    {
        if (stats.CharacterType == m_characterType)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void GetCharacterTypeFromParent()
    {
        if (TryGetComponentInParent(out EnemyStats enemy))
        {
            m_characterType = CharacterType.Enemy;
        }
        else if (TryGetComponentInParent(out PlayerStats player))
        {
            m_characterType = CharacterType.Player;
        }
        else
        {
            Debug.LogWarning("Spell should have a parent object with either a EnemyStats or PlayerStats");
        }
    }

    public bool TryGetComponentInParent<T>(out T component)
    {
        component = transform.GetComponentInParent<T>();

        if (component != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
