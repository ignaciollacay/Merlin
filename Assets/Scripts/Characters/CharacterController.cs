using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Parent Character Controller.
/// Characters give damage, take damage & die 
/// </summary>
public abstract class CharacterController : MonoBehaviour
{
    public UnityEvent OnCharacterAttack;
    public UnityEvent OnCharacterDamaged;

    public Animator animator;

    // Both Player & Enemies should play an animation when attacking or recieving Damage
    // TODO: Player should have animations for attack and recieve damage (camera movements)

    public virtual void Attack()
    {
        OnCharacterAttack?.Invoke();
    }

    public virtual void RecieveAttack()
    {
        OnCharacterDamaged?.Invoke();
    }

    public abstract void Death();

    private void OnDisable()
    {
        OnCharacterAttack.RemoveAllListeners();
        OnCharacterDamaged.RemoveAllListeners();
    }
}
