using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpellcastType
{
    attack,
    defense
}

[CreateAssetMenu(fileName = "New Spell", menuName = "SO/Spell")]
public class SpellSO : ScriptableObject
{
    [Header("General Properties")]
    public string spellName;
    [TextArea(5, 10)]
    public string Spell;
    [TextArea(5, 10)]
    public string description = "";
    [Tooltip("The object spawned when the spell is casted")]
    public GameObject prefab;


    [Header("Cast Properties")]
    [Tooltip("Type of spell. Attack or Defense?")]
    public SpellcastType type;
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


    // Added on refactorization of ItemSpells. Linked to Object Animation on SpellCasted, on Dojo Scene
    // TODO: Refactor behavior.
    public string boolName;




}
