using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HechizoDefensa : MonoBehaviour
{
    public int duration;
    public SpellSO spell;

    private PlayerStats playerStats;
    private int baseStat;
    private int newStat;

    ParticleSystem vfx;

    void Awake()
    {
        vfx = GetComponent<ParticleSystem>();
        playerStats = GetComponentInParent<PlayerStats>();

        baseStat = playerStats.defense;
        newStat = baseStat + spell.defense;
    }

    private void Start()
    {
        StartCoroutine(Defend());
    }

    private IEnumerator Defend()
    {
        yield return new WaitUntil(() => vfx.isPlaying);
        //print("player defending started. Defense=" + playerStats.defense);
        playerStats.defense = newStat;
        yield return new WaitForSeconds(duration);
        playerStats.defense = baseStat;
        vfx.Stop();
        //print("player defending stopped. Defense=" + playerStats.defense);
        StartCoroutine(Defend());
    }

    private void OnDisable()
    {
        playerStats.defense = baseStat;
        StopCoroutine(Defend());
    }
}
