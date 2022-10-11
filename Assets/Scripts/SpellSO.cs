using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellcraftType
{
    ingredient,
    spell
}

// Me la complico para que sea automática la generación de spells
// Hay que ver cómo van a ser los spells, no sé la posición si es siempre al principio
[CreateAssetMenu(fileName = "New Spell", menuName = "SO/Spell")]
public class SpellSO : ScriptableObject
{
    public string spellName;

    [Tooltip("Spell Phrase, without the ingredients")]
    public string SpellString;
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

    public int damage;


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
