using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    [SerializeField] EnemyStats selectedEnemy;
    [SerializeField] float m_Speed = 0.25f;
    Vector3 lTargetDir;

    private void Start()
    {
        if (selectedEnemy == null)
            AttackNextEnemy();
        else
        {
            selectedEnemy.OnEnemyKilled.AddListener(AttackNextEnemy);
            lTargetDir = selectedEnemy.transform.position - transform.position;
            lTargetDir.y = 0.0f;
        }
            
    }

    void SelectEnemy()
    {
        selectedEnemy = FindObjectOfType<EnemyStats>();
        if (selectedEnemy != null)
        {
            selectedEnemy.OnEnemyKilled.AddListener(AttackNextEnemy);
            Debug.Log("selecting new enemy=" + selectedEnemy.name, selectedEnemy.gameObject);
        }
    }
    
    async void AttackNextEnemy()
    {
        await System.Threading.Tasks.Task.Delay(1500);
        SelectEnemy();
        if (selectedEnemy != null)
        {
            lTargetDir = selectedEnemy.transform.position - transform.position;
            lTargetDir.y = 0.0f;
        }
    }


    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * m_Speed);
    }
}
