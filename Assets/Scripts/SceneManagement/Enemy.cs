using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;

    public static Enemy CreateEnemy(EnemySO enemySO)
    {
        GameObject obj = Instantiate(enemySO.prefab);
        Enemy enemy = obj.AddComponent<Enemy>();
        enemy.stats = obj.GetComponent<EnemyStats>();
        return enemy;
    }
}