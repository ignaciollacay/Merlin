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
    public Sprite b_spriteEnabled;
    public Sprite b_spriteDisabled;
    public bool cooldown;
    public int count;
    public RadialBar cooldownBar;

    /// <summary>
    /// Creates a spell gameobject storing all the data in the SO's.
    /// </summary>
    /// <param name="spell">SpellSO to be instantiated</param>
    /// <param name="spawnPos">Spell spawn Position and Rotation</param>
    /// <param name="button">Button assigned to the Spell</param>
    /// <returns>Spell SO, SO properties, GO components, UI button and count</returns>
    public static CastedObject Create(SpellSO spell, Transform spawnPos, Button button)
    {
        GameObject obj = Instantiate(spell.result.prefab, spawnPos);

        #region GetComponent (Obsolete)
        // Switching to AddComponent instead of GetComponent.
        // GetComponent Requires to add a component to each spell prefab.
        // GetComponent Only useful if SpellSO data where Serialized into the prefab instead of generated on runtime. 
        //CastedObject castedObject = transform.gameObject.GetComponent<CastedObject>();
        #endregion
        CastedObject castedObject = obj.AddComponent<CastedObject>();

        castedObject.SO = spell;
        castedObject.vfx = castedObject.GetComponent<ParticleSystem>();
        castedObject.sfx = castedObject.GetComponent<AudioSource>();
        castedObject.damage = spell.damage;
        //castedObject.collision = castedObject.gameObject.AddComponent<SpellCollision>();
        //castedObject.collision.spellDamage = spell.damage;
        castedObject.button = button;
        castedObject.b_spriteEnabled = spell.buttonEnabled;
        castedObject.b_spriteDisabled = spell.buttonDisabled;
        castedObject.b_image = button.GetComponent<Image>();
        castedObject.cooldownBar = button.GetComponentInChildren<RadialBar>();

        castedObject.b_image.sprite = spell.buttonEnabled;
        castedObject.cooldownBar.ring.sprite = spell.buttonDisabled;

        return castedObject;
    }

    public async void Cooldown()
    {
        cooldown = true;
        button.interactable = false;
        cooldownBar.ring.enabled = true;
        cooldownBar.cooldownTime = SO.cooldown;
        cooldownBar.cooldownBool = true;
        await System.Threading.Tasks.Task.Delay(SO.cooldown * 1000);
        cooldown = false;
        button.interactable = true;
        cooldownBar.ring.enabled = false;
        //button.image.sprite = selected;
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
