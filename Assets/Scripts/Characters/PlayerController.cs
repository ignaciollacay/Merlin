using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public override void Death()
    {
        Debug.LogWarning("Death Anim Not Implemented");
    }
}
