using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpells : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public AnimationClip spellColor;
    public AnimationClip spellLevitate;
    public AnimationClip spellDisappear;
    public AnimationClip spellFire;

    [SerializeField]
    private CastManager castManager;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        castManager.OnSpellCasted += CastSpell;
    }

    void CastSpell(SpellSO spell)
    {
        switch (spell.name)
        {
            case ("SpellColor"):
                SpellColor();
                break;
            case ("SpellLevitate"):
                SpellLevitate();
                break;
            case ("SpellDisappear"):
                SpellDisappear();
                break;
            case ("SpellFire"):
                SpellFire();
                break;

            default:
                break;
        }
    }

    void SpellColor()
    {
        animator.SetBool("Color", true);
    }
    void SpellLevitate()
    {
        animator.SetBool("Levitate", true);
    }
    void SpellDisappear()
    {
        animator.SetBool("Disappear", true);
    }
    void SpellFire()
    {
        animator.SetBool("Fire", true);
    }
}
