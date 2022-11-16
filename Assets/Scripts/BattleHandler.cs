using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Counter))]
public class BattleHandler : MonoBehaviour
{
    private Counter counter;

    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleEnd;

    void Awake()
    {
        counter = GetComponent<Counter>();

        OnBattleStart?.Invoke();
    }

    public void AddEnemy() => counter.Increase();

    public void RemoveEnemy() => counter.Decrease();

    public void StartEndBattleRoutine() => StartCoroutine(EndBattleRoutine());

    public IEnumerator EndBattleRoutine()
    {
        yield return new WaitForSeconds(2);

        while (counter.BiggerThan(0))
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);

        EndBattle();
    }

    private void EndBattle()
    {
        OnBattleEnd?.Invoke();
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        DialogueManager.Instance.DialogueEnd.AddListener(sceneHandler.LoadAsync);
    }
}