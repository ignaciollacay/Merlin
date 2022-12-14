using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    private EnemyStats selectedEnemy;
    [SerializeField] float m_Speed = 0.25f;
    Vector3 lTargetDir;

    private void Start()
    {
        AttackNextEnemy();
    }

    void SelectEnemy()
    {
        selectedEnemy = FindObjectOfType<EnemyStats>();
        if (selectedEnemy != null)
        {
            selectedEnemy.OnEnemyKilled.AddListener(AttackNextEnemy);
        }
    }
    
    async void AttackNextEnemy()
    {
        await System.Threading.Tasks.Task.Delay(1500); // Wait for the enemy to be destroyed
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
