using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MagicController : MonoBehaviour
{
    [Header("Magic Spell")]
    public Transform spawnPos;
    public int amountOfShots = 5;

    [SerializeField] private CastManager castManager;

    [Header("Character Stats")]
    [SerializeField] private PlayerStats playerStats; // TODO run via event subscription in PlayerStats

    // Spell variables
    [SerializeField] private Button[] buttons;

    private CastedObject[] castedObject = new CastedObject[4];

    // Not used
    //public delegate void SpellFired(SpellSO spellFired);
    //public event SpellFired OnSpellFired;

    private void Start()
    {
        castManager.OnSpellCasted += CastSpell; // TODO. Es necesario? Ver tema Refactorizacion de Magic Controller y Cast Manager. Pueden ser uno.
    }

    private void CastSpell(SpellSO spell)
    {
        int slot = GetSlot(spell);
        castedObject[slot] = CastedObject.Create(spell, spawnPos, buttons[slot]);
    }

    public void ShootSpell(int slot)
    {
        CastedObject spell = castedObject[slot];
        if (!spell.cooldown)
        {
            spell.vfx.Play();
            spell.sfx.Play();
            spell.Cooldown();
            // TODO: refactor and decouple Mana dependencies
            //playerStats.UseMana(spell.SO.mana);

            // Check if using count
            if (spell.SO.maxCount != 0)
            {
                // Add count
                spell.counter.Increase();

                // Reset button if no remaining shots.
                if (spell.counter.BiggerOrEqualThan(spell.SO.maxCount))
                {
                    spell.b_image.sprite = null;
                    Destroy(spell.gameObject);
                }
            }
        }
    }
    private int GetSlot(SpellSO spell)
    {
        switch (spell.type)
        {
            case SpellType.Attack:
                if (castedObject[0] == null)
                    return 0;
                else if (castedObject[1] == null)
                    return 1;
                else if (castedObject[2] == null)
                    return 2;
                else
                {
                    Debug.Log("Spell Slots are full, new spell could not be added.");
                    return 8;
                }
            case SpellType.Defense:
                return 3;
            default:
                Debug.Log("SpellType not recognized");
                return 9;
        }
    }
}
