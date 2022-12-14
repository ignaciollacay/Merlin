using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpellHighlight : MonoBehaviour
{
    private ParticleSystem vfx;

    private void Awake()
    {
        vfx = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        StartCoroutine(HighlightNewItemCoroutine());
    }

    IEnumerator HighlightNewItemCoroutine()
    {
        // Wait until the animation of the parent ends. Otherwise it will play while hidden offscreen.
        yield return new WaitForSeconds(2); 
        vfx.Play();
        yield return new WaitUntil(() => vfx.isStopped);
        yield return new WaitUntil(() => vfx.particleCount == 0);
        Destroy(gameObject);
    }
}
