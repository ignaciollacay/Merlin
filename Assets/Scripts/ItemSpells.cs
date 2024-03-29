using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// FIXME: Crossreferences & learned spell function
public class ItemSpells : MonoBehaviour
{
    private Animator animator;

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
        yield return new WaitUntil(() => animator.GetBool(spell.boolName) == false);
        OnAnimationEnd?.Invoke(spell);
    }

    public void Animate()
    {
        animator.SetBool(spell.boolName, true);
        StartCoroutine(AnimEnd());
    }

    public void AssignSpellSO(SpellSO _spell)
    {
        spell = _spell;
    }
}
