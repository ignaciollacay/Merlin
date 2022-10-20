using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpells : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private CastManager castManager;

    
    public delegate void SpellLearned(SpellSO spell);
    /// <summary>
    ///     Follows after OnSpellCasted. Allows to do things after the animation has been played
    /// </summary>
    public event SpellLearned OnSpellLearned;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
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
    }
}
