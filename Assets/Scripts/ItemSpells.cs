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

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //castManager.OnSpellCasted += Animate;

        //AssessmentHandler.OnAssessmentStart += AssignSpellSO;
    }

    private IEnumerator AnimEnd(SpellSO spell)
    {
        yield return new WaitUntil(() => animator.GetBool(spell.boolName) == false);
        OnAnimationEnd?.Invoke(spell);
    }

    public void Animate(SpellSO spell)
    {
        var boolName = Assignment.Instance.currentAssignment.boolName;
        animator.SetBool(boolName, true);
        StartCoroutine(AnimEnd(spell));
        //spellSFX.Play(); // Obsolete. Played from timeline
    }

    private SpellSO spell;

    public void Animate()
    {
        print("Signal recieved. Playing Animate()");
        animator.SetBool(spell.boolName, true);
        StartCoroutine(AnimEnd(spell));
        //spellSFX.Play(); // FIXME: Refactor OnSpellCasted to UnityEvent & call audio source to play by inspector? 
    }

    public void AssignSpellSO(SpellSO _spell)
    {
        spell = _spell;
        print("Assigned spell" + spell);
    }
}
