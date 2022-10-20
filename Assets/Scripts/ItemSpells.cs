using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpells : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField]
    private Animator animator;

    public AnimationClip spellColor;
    public AnimationClip spellLevitate;
    public AnimationClip spellDisappear;
    public AnimationClip spellFire;

    [SerializeField]
    private CastManager castManager;

=======
    private Animator animator;

    [SerializeField] private CastManager castManager;

    
    public delegate void SpellLearned(SpellSO spell);
    /// <summary>
    ///     Follows after OnSpellCasted. Allows to do things after the animation has been played
    /// </summary>
    public event SpellLearned OnSpellLearned;
>>>>>>> Stashed changes

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
<<<<<<< Updated upstream
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
=======
        castManager.OnSpellCasted += Animate;
    }

    private IEnumerator AnimEnd(SpellSO spell)
    {
        yield return new WaitUntil(() => animator.GetBool(spell.boolName) == false);
        OnSpellLearned.Invoke(spell);
    }

    void Animate(SpellSO spell)
    {
        animator.SetBool(spell.boolName, true);
        StartCoroutine(AnimEnd(spell));
>>>>>>> Stashed changes
    }
}
