using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Very similar to Ingredient
// Can be refactored with inheritance
// Replaces SO type and Selection method

[RequireComponent(typeof(Text))]
public class Spell : MonoBehaviour
{
    public SpellSO spellSO;
    public Text text;

    public virtual void Awake()
    {
        text = GetComponent<Text>();
        text.text = spellSO.spellName + ": " + spellSO.Spell;
    }
}
