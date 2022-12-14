using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellcraftType
{
    ingredient,
    spell
}

public enum SpellcastType
{
    attack,
    defense
}

// Me la complico para que sea automática la generación de spells
// Hay que ver cómo van a ser los spells, no sé la posición si es siempre al principio
[CreateAssetMenu(fileName = "New Spell", menuName = "SO/Spell")]
public class SpellSO : ScriptableObject
{
    public string spellName;

    [Tooltip("Spell Phrase, without the ingredients")]
    public string SpellString;
    [TextArea(5, 10)]
    public string Spell;
    private string spell;
    [Tooltip("Input Items required for spell")]
    public ItemSO[] ingredients = new ItemSO[2];

    // FIXME acá hay un tema. No siempre es un itemSO. Puede ser un Spell... Diferenciamos?
    [Tooltip("Output Item produced by spell")]
    public ItemSO result;

    //[Tooltip("Output Spell produced by spell")]
    //public SpellSO resultSpell;

    [TextArea(5, 10)]
    public string description = "";

    [Header("Spell Properties")]
    [Tooltip("The amount of mana consumed when fired (or casted?)")]
    public int mana;
    [Tooltip("The damage value taken by enemy on particle collision")]
    public int damage;
    [Tooltip("The defense value given to player stats while playing (linked to duration value on HechizoDefensa)")]
    public int defense;
    [Tooltip("The amount of time to wait between fires")]
    public int cooldown;
    [Tooltip("The icon displayed in the inventory")]
    public Sprite icon;
    [Tooltip("The icon displayed in the button")]
    public Sprite buttonEnabled;
    [Tooltip("The icon displayed in the button when on Cooldown")]
    public Sprite buttonDisabled;
    [Tooltip("The maximum amount of times the spell can be fired before being destroyed")]
    public int count;

    // TODO adding to diferentiate between learned or unlearned, due to incompatibilities caused by Craft, SpellBook & SceneManager.
    [Tooltip("If the spell is unlocked or needs to be learned")]
    public bool learned = false;

    // Added to allow assignment of different buttons according to the spell type (Right-Attack, Left-Defense)
    [Tooltip("Type of spell. Attack or Defense?")]
    public SpellcastType type;


    // Added on refactorization of ItemSpells.
    // Linked to Object Animation on SpellCasted, on Dojo Scene
    public string boolName;

    [ContextMenu("Generate Spell")]
    public void AutoGenerate()
    {
        foreach (var item in ingredients)
        {
            if (item != null)
            {
                Spell = GetSpellString();
            }
            else
            {
                //Spell = GetSpellString();
                Debug.LogWarning("Missing craftable");
            }
        }
    }

    string GetSpellString()
    {
        spell = ingredients[0].itemName + ", " + ingredients[1].itemName + ", " + SpellString;
        return spell;
    }
}
