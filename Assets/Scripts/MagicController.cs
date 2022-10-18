using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MagicController : MonoBehaviour
{
    [Header("Magic Spell")]
    public Transform spawnPos;
    public int amountOfShots = 5;

    [SerializeField] private CastManager castManager;

    [Header("Character Stats")]
    [SerializeField] private PlayerStats playerStats;

    // Spell variables
    private int spellCount;
    [SerializeField] private Button[] buttons;
    private GameObject[] spellGOs = new GameObject[3];
    private SpellSO[] spellSOs = new SpellSO[3];
    private ParticleSystem[] spellVFXs = new ParticleSystem[3];
    private bool[] spellCooldowns = new bool[3];
    private int[] spellShots = new int[3];


    // accessor + ClassType[delegate] + returnType + ClassName + ("parameters");
    public delegate void SpellFired(SpellSO spellFired);
    // accessor + ClassType[event] + Class[Delegate] + EventName ["-On + Class[delegate]";]
    public event SpellFired OnSpellFired;

    private void Start()
    {
        castManager.OnSpellCasted += CastSpell; // TODO Ver tema Refactorizacion de Magic Controller y Cast Manager. Pueden ser uno.
    }

    private void CastSpell(SpellSO spell)
    {
        int slot = GetEmptySpellSlot();
        // Create crafted item
        spellGOs[slot] = Instantiate(spell.result.prefab, spawnPos.position, spawnPos.rotation, spawnPos);
        spellSOs[slot] = spell;
        spellVFXs[slot] = spellGOs[slot].GetComponent<ParticleSystem>();

        // Add icon to button
        buttons[slot].gameObject.GetComponent<Image>().sprite = spell.icon;
    }

    public void ShootSpell(int slot)
    {
        if (!spellCooldowns[slot])
        {
            spellVFXs[slot].Play();
            Cooldown(slot);
            spellShots[slot]++;

            playerStats.UseMana(spellSOs[slot].mana);
        }

        if (spellShots[slot] >= amountOfShots)
        {
            // destroy / discard?
            Destroy(spellGOs[slot]);
            buttons[slot].gameObject.GetComponent<Image>().sprite = null;
            spellShots[slot] = 0;
            Debug.Log("Max Amount of spell shots");
        }
    }

    async void Cooldown(int slot)
    {
        spellCooldowns[slot] = true;
        buttons[slot].interactable = false;
        //Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
        await Task.Delay(spellSOs[slot].cooldown * 1000);
        spellCooldowns[slot] = false;
        buttons[slot].interactable = true;
        //Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
    }

    private int GetEmptySpellSlot()
    {
        if (spellGOs[0] == null)
        {
            return 0;
        }
        else if (spellGOs[1] == null)
        {
            return 1;
        }
        else if (spellGOs[2] == null)
        {
            return 2;
        }
        else
        {
            Debug.Log("Spell Slots are full, new spell could not be added.");
            return 9;
        }
    }
}


public class MagicController2 : MonoBehaviour
{
    [Header("Magic Spell")]
    public Transform spawnPos;
    public int amountOfShots = 5;

    [SerializeField] private CastManager castManager;

    [Header("Character Stats")]
    [SerializeField] private PlayerStats playerStats; // TODO run via event subscription in PlayerStats

    // Spell variables
    [SerializeField] private Button[] buttons;


    public delegate void SpellFired(SpellSO spellFired);
    public event SpellFired OnSpellFired;

    private void Start()
    {
        castManager.OnSpellCasted += CastSpell; // TODO. Es necesario? Ver tema Refactorizacion de Magic Controller y Cast Manager. Pueden ser uno.
    }

    CastedObject[] castedObject = new CastedObject[3];

    private void CastSpell(SpellSO spell)
    {
        int slot = GetEmptySpellSlot();
        castedObject[slot] = CastedObject.Create(spell, spawnPos, buttons[slot]);
    }

    public void ShootSpell(int slot)
    {
        CastedObject spell = castedObject[slot];
        if (!spell.cooldown)
        {
            spell.vfx.Play();
            spell.Cooldown();
            spell.count++;

            playerStats.UseMana(spell.SO.mana);

            spell.CheckCount();
        }
    }

    private int GetEmptySpellSlot()
    {
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
            return 9;
        }
    }
}

public class CastedObject : MonoBehaviour
{
    public SpellSO SO;
    public ParticleSystem vfx;
    public int damage;
    public SpellCollision collision;
    public Button button;
    public Image b_image;
    public bool cooldown;
    public int count;

    public static CastedObject Create(SpellSO spell, Transform spawnPos, Button button)
    {
        Transform transform = Instantiate(spell.result.prefab.transform, spawnPos.position, spawnPos.rotation);

        CastedObject castedObject = transform.GetComponent<CastedObject>();

        castedObject.SO = spell;
        castedObject.vfx = castedObject.GetComponent<ParticleSystem>();
        castedObject.damage = spell.damage;
        castedObject.collision = castedObject.gameObject.AddComponent<SpellCollision>();
        castedObject.collision.spellDamage = spell.damage;
        castedObject.button = button;
        castedObject.b_image = button.GetComponent<Image>();

        castedObject.b_image.sprite = spell.icon;

        return castedObject;
    }

    public async void Cooldown()
    {
        cooldown = true;
        button.interactable = false;
        //Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
        await Task.Delay(SO.cooldown * 1000);
        cooldown = false;
        button.interactable = true;
        //Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
    }

    public void CheckCount()
    {
        if (count >= SO.count)
        {
            b_image.sprite = null;
            Destroy(gameObject);
            Debug.Log("Max spell count fired");
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
