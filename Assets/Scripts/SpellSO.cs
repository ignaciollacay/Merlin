using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "SO/Spell")]
public class SpellSO : ScriptableObject
{
    public int id;
    public string Name;
    [TextArea(3, 10)] 
    public string phrase;
    public SpellType type; // TODO: Convert into SO? 
    public int value;
    public int cooldown;
    [TextArea(5, 10)] 
    public string description;

    public GameObject prefab;
    [Tooltip("The icon displayed in the button")]
    public Sprite icon;
    [Tooltip("The maximum amount of times the spell can be fired before being destroyed. 0 does not use counter")]
    public int maxCount = 0;


    // Added on refactorization of ItemSpells. Linked to Object Animation on SpellCasted, on Dojo Scene
    public string boolName; // TODO: Remove 
}

