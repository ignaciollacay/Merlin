using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesToggle : MonoBehaviour
{
    [SerializeField] private ParticleSystem vfx;

    private bool _playing;

    public void ToggleParticles()
    {
        if (!_playing)
        {
            vfx.Play();
            _playing = true;
        }
        else
        {
            vfx.Stop();
            _playing = false;
        }
    }

    public void ResetToggleState()
    {
        _playing = false;
    }
}
