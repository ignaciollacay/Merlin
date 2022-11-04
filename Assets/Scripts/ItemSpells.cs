using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// FIXME: Crossreferences & learned spell function
public class ItemSpells : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private AudioSource spellSFX;
    [SerializeField] private CastManager castManager;

    /// <summary>
    ///     Follows after OnSpellCasted. Allows to do things after the animation has been played
    /// </summary>
    public UnityEvent<SpellSO> OnAnimationEnd;

    private SpellSO spell;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator AnimEnd()
    {
        print("Animation ended " + spell.spellName);
        yield return new WaitUntil(() => animator.GetBool(spell.boolName) == false);
        OnAnimationEnd?.Invoke(spell);
    }

    public void Animate()
    {
        print("Animating..." + spell.boolName);
        animator.SetBool(spell.boolName, true);
        StartCoroutine(AnimEnd());
    }

    public void AssignSpellSO(SpellSO _spell)
    {
        spell = _spell;
        print("Assigned spell " + spell.spellName);
    }
}
