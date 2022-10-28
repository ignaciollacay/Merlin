using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Very similar to Ingredient
// Can be refactored with inheritance
// Replaces SO type and Selection method

// TODO UPDATE Now parent of SpellCast & SpellCraft
// Not yet implemented. Waiting till system finished.

// Used for SpellBook

[RequireComponent(typeof(Text))]
public class SpellScroll : Spell
{
    public Text text;

    public virtual void Awake()
    {
        text = GetComponent<Text>();
        text.text = spellSO.spellName + ": " + spellSO.Spell;
    }
}
