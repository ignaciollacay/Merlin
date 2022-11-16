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

    [Header("Character Stats")]
    [SerializeField] private PlayerStats playerStats; // TODO: run via event subscription in PlayerStats

    // Spell variables
    [SerializeField] private Button[] buttons = new Button[4];

    private CastedObject[] castedObject = new CastedObject[4];

    [SerializeField] private Sprite buttonEmptyContainer; // FIXME: Hide Button when empty 

    public List<SpellSO> spells = new List<SpellSO>();

    private void Start()
    {
        if (buttonEmptyContainer == null)
            buttonEmptyContainer = buttons[0].image.sprite;

        foreach (var spell in spells)
        {
            CastSpell(spell);
        }
    }

    public void CastSpell(SpellSO spell)
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
            spell.count++;

            // FIXME: Refactor. Decouple Mana from this script
            //playerStats.UseMana(spell.SO.mana); 

            //spell.CheckCount();
        }
    }
    private int GetSlot(SpellSO spell)
    {
        switch (spell.type)
        {
            case SpellcastType.attack:
                if (castedObject[0] == null)
                    return 0;
                else if (castedObject[1] == null)
                    return 1;
                else
                {
                    Debug.Log("Attack Spell Slots are full, new spell could not be added.");
                    return 8;
                }

            case SpellcastType.defense:
                if (castedObject[2] == null)
                    return 2;
                else if (castedObject[2] == null)
                    return 3;
                else
                {
                    Debug.Log("Defense Spell Slots are full, new spell could not be added.");
                    return 8;
                }

            default:
                Debug.Log("SpellType not recognized");
                return 9;
        }
    }
    public void Clear(int i)
    {
        castedObject[i].b_image.sprite = buttonEmptyContainer;
        Destroy(castedObject[i].gameObject);
    }
    public void ClearAll()
    {
        for (int i = 0; i < castedObject.Length; i++)
        {
            castedObject[i].b_image.sprite = buttonEmptyContainer;
            Destroy(castedObject[i].gameObject);
        }
    }
    public void SetAll(InventorySpellSO inventory)
    {
        foreach (var spell in inventory.spells)
        {
            spells.Add(spell);
        }
    }
}
