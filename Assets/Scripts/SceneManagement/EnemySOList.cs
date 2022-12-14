using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy List", menuName = "SO/Enemy/Enemy List")]
public class EnemySOList : ScriptableObject
{
    public List<EnemySO> enemyList;
}
