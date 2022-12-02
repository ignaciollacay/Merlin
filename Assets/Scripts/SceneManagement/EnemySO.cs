using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "SO/Enemy/Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject prefab;
    public Transform spawnPosition { get => prefab.transform; } // TODO: Allow to override the prefab transform.

}
