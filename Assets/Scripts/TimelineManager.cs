using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [Header("Timeline References")]
    public PlayableDirector playableDirector;

    [SerializeField] private PhraseRecognition phraseRecognition;

    private void Start()
    {
        phraseRecognition.OnPhraseRecognized += PlayTimeline;
    }
    public void PlayTimeline()
    {
        playableDirector.Play();
    }

    private void OnDisable()
    {
        phraseRecognition.OnPhraseRecognized -= PlayTimeline;
    }
}