using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    [Header("Magic Spell")]
    public Transform spawnPos;

    [SerializeField] private CastManager castManager;

    private GameObject spell;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;


    private void Start()
    {
        castManager.OnSpellCasted += CastSpell;
    }

    private void CastSpell(SpellSO spellSO)
    {
        // Destroy previous spell
        Destroy(spell);

        // Create crafted item
        spell = Instantiate(spellSO.result.prefab, spawnPos.position, spawnPos.rotation, spawnPos);

        // Allow to throw spell multiple times.
            //craftVFX.Play();

        // Take damage
            // Enemy TakeDamage run via OnSpellCasted event subscription in EnemyStats
            enemyStats.TakeDamage(spellSO.damage);


    }

    // TODO Destroy VFX Asset after use.
}
