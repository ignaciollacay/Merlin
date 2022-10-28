using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCard : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [Header("Spell Values")]
    [SerializeField] private Text spellName;
    [SerializeField] private Image icon;
    [SerializeField] private Text typeText;
    [SerializeField] private Text type;
    [SerializeField] private Text value; //damage or defense
    [SerializeField] private Text mana;
    [SerializeField] private Text cooldown;

    [Tooltip("Interactive object being animated with spellcast")]
    [SerializeField] ItemSpells item;

    private void Start()
    {
        item.OnSpellLearned += DisplaySpellCard; // TODO necesitaria un delay, para esperar a que termine la animacion
    }

    async void DisplaySpellCard(SpellSO spell)
    {
        SetSpellStats(spell);
        canvas.enabled = true;
        // Play intro anim
        await System.Threading.Tasks.Task.Delay(2500);
        // Play outro anim
        canvas.enabled = false;
    }

    void SetSpellStats(SpellSO spell)
    {
        spellName.text = spell.Name;
        icon.sprite = spell.icon;
        type.text = spell.type.ToString();
        value.text = "+" + spell.value;
        // TODO: Decouple mana logic from SpellSO? ManaUser interface or independent class?
        //mana.text = spell.mana.ToString();
        cooldown.text = cooldown.ToString();
    }
}