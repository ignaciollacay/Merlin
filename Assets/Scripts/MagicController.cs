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

    private Sprite buttonImage;

    private void Start()
    {
        buttonImage = buttons[0].image.sprite;
        castManager.OnSpellCasted += CastSpell; // TODO. Es necesario? Ver tema Refactorizacion de Magic Controller y Cast Manager. Pueden ser uno.
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

            playerStats.UseMana(spell.SO.mana);

            spell.CheckCount();
        }
    }
    private int GetSlot(SpellSO spell)
    {
        switch (spell.type)
        {
            case SpellcastType.attack:
                if (castedObject[0] == null)
                {
                    return 0;
                }
                else if (castedObject[1] == null)
                {
                    return 1;
                }
                else if (castedObject[2] == null)
                {
                    return 2;
                }
                else
                {
                    Debug.Log("Spell Slots are full, new spell could not be added.");
                    return 8;
                }
            case SpellcastType.defense:
                return 3;
            default:
                Debug.Log("SpellType not recognized");
                return 9;
        }
    }
    public void Clear(int i)
    {
        castedObject[i].b_image.sprite = buttonImage;
        Destroy(castedObject[i].gameObject);
    }
    public void ClearAll()
    {
        for (int i = 0; i < castedObject.Length; i++)
        {
            castedObject[i].b_image.sprite = buttonImage;
            Destroy(castedObject[i].gameObject);
        }
    }

    public void SetAll()
    {

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
            Clear(0);
        if (Input.GetKey(KeyCode.Alpha1))
            Clear(1);
        if (Input.GetKey(KeyCode.Alpha2))
            Clear(2);
        if (Input.GetKey(KeyCode.Alpha3))
            Clear(3);
        if (Input.GetKey(KeyCode.Alpha4))
            Clear(4);
        if (Input.GetKey(KeyCode.Alpha9))
            ClearAll();
    }
}
