using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //desprolijo, sacar de este script -- TBD

public class PlayerStats : CharacterStats
{
    public int life = 100;


    private void Start()
    {
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitUntil(() => life <= 0);
        Death();
    }

    public override void Death()
    {
        base.Death();

        ResetScene();
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
