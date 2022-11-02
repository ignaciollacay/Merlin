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
    public UnityEvent<SpellSO> OnSpellLearn;

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
        OnSpellLearn.Invoke(spell);
    }

    public void Animate(SpellSO spell)
    {
        animator.SetBool(spell.boolName, true);
        StartCoroutine(AnimEnd(spell));
        spellSFX.Play(); // FIXME: Refactor OnSpellCasted to UnityEvent & call audio source to play by inspector? 
    }
}
