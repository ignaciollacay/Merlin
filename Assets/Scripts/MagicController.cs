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
    [SerializeField] private EnemyStats enemyStats;

    // Spell variables
    private int spellCount;
    [SerializeField] private Button[] buttons;
    //[SerializeField] private Button button;
    private GameObject[] spellGOs = new GameObject[3];
    //private GameObject spellGO;
    private SpellSO[] spellSOs = new SpellSO[3];
    //private SpellSO spellSO;
    private ParticleSystem[] spellVFXs = new ParticleSystem[3];
    //private ParticleSystem spellVFX;
    private bool[] spellCooldowns = new bool[3];
    //private bool spellCooldown = false;
    private int[] spellShots = new int[3];
    //private int spellShot = 0;

    private void Start()
    {
        castManager.OnSpellCasted += CastSpell;
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

            // Damage - Enemy TakeDamage run via OnSpellCasted event subscription in EnemyStats
            enemyStats.TakeDamage(spellSOs[slot].damage);
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
        Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
        await Task.Delay(spellSOs[slot].cooldown * 1000);
        spellCooldowns[slot] = false;
        buttons[slot].interactable = true;
        Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
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
