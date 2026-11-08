using UnityEngine;
using System.Collections.Generic;
public class StageRoom : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private StageTeleporter teleporter;

    public Transform PlayerSpawn => playerSpawn;
    public List<Transform> EnemySpawnPoints => spawnPoints;
}
