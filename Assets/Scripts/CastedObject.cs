using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CastedObject : MonoBehaviour
{
    public SpellSO SO;
    public ParticleSystem vfx;
    public AudioSource sfx;
    public int damage;
    //public SpellCollision collision;
    public Button button;
    public Image b_image;
    public bool cooldown;
    public Counter counter;

    /// <summary>
    /// Creates a spell gameobject storing all the data in the SO's.
    /// </summary>
    /// <param name="spell">SpellSO to be instantiated</param>
    /// <param name="spawnPos">Spell spawn Position and Rotation</param>
    /// <param name="button">Button assigned to the Spell</param>
    /// <returns>Spell SO, SO properties, GO components, UI button and count</returns>
    public static CastedObject Create(SpellSO spell, Transform spawnPos, Button button)
    {
        GameObject obj = Instantiate(spell.prefab, spawnPos);

        CastedObject castedObject = obj.AddComponent<CastedObject>();

        castedObject.SO = spell;
        castedObject.vfx = castedObject.GetComponent<ParticleSystem>();
        castedObject.sfx = castedObject.GetComponent<AudioSource>();
        castedObject.damage = spell.value;
        // Spell Collision added to prefab to distinguish which VFX will collide.
        //castedObject.collision = castedObject.gameObject.AddComponent<SpellCollision>();
        //castedObject.collision.spellDamage = spell.damage;
        castedObject.button = button;
        castedObject.b_image = button.GetComponent<Image>();

        castedObject.b_image.sprite = spell.icon;

        if (spell.maxCount != 0)
            castedObject.counter = obj.AddComponent<Counter>();

        return castedObject;
    }

    public async void Cooldown()
    {
        cooldown = true;
        button.interactable = false;
        //Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
        await System.Threading.Tasks.Task.Delay(SO.cooldown * 1000);
        cooldown = false;
        button.interactable = true;
        //Debug.Log("Button " + slot + " / Cooldown=" + spellCooldowns[slot]);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
