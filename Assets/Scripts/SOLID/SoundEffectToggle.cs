using UnityEngine;

public class SoundEffectToggle : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip toggleOn;
    [SerializeField] private AudioClip toggleOff;

    private bool _playing;

    public void ToggleSoundEffect()
    {
        if (!_playing)
        {
            audioSource.clip = toggleOn;
            audioSource.Play();
            _playing = true;
        }
        else
        {
            audioSource.clip = toggleOff;
            audioSource.Play();
            _playing = false;
        }
    }

    public void ResetToggleState()
    {
        _playing = false;
    }
}