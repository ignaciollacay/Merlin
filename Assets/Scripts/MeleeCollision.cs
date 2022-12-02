using UnityEngine;

public class MeleeCollision : SpellCollision
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Attack collided");
        GameObject other = collision.gameObject;
        SpellCollided(other);
    }
}
