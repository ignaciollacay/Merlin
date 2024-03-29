using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : CharacterStats
{
    public override CharacterType CharacterType { get; set; } = CharacterType.Player;

    public UnityEvent OnPlayerKilled;

    public override void Death()
    {
        base.Death();

        ResetScene();
    }

    // TODO run through event? Parent OnCharacterDeath?
    private void ResetScene()
    {
        OnPlayerKilled?.Invoke();
    }

}
