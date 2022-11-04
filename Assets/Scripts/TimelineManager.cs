using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public void PlayTimeline(PlayableDirector playableDirector)
    {
        playableDirector.RebuildGraph();
        playableDirector.Play();
    }
}