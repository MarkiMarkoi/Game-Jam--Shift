using UnityEngine;
using System.Collections.Generic;
public class StageRoom : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private StageTeleporter teleporter;

    private int remainingEnemies = 0;
    public Transform PlayerSpawn => playerSpawn;
    public List<Transform> EnemySpawnPoints => spawnPoints;

    public void RegisterEnemy(Health enemyHealth)
    {
        if (enemyHealth != null) 
        {
            remainingEnemies++;
            enemyHealth.OnDeath += HandleEnemyDeath;
        }
    }
    private void HandleEnemyDeath()
    {
        remainingEnemies--;
        Debug.Log("Remaining Enemies: " + remainingEnemies);
        FinishSpawning();
    }

    public void FinishSpawning()
    {
        if(remainingEnemies == 0)
        {
            teleporter.Unlock();
        }
    }
}
