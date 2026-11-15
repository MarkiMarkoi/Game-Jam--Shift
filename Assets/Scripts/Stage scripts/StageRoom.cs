using UnityEngine;
using System.Collections.Generic;
using System;
public class StageRoom : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private StageTeleporter teleporter;

    private int remainingEnemies = 0;
    public Transform PlayerSpawn => playerSpawn;
    public int RemainingEnemies => remainingEnemies;

    public event Action OnEnemyCountChanged;
    public List<Transform> EnemySpawnPoints => spawnPoints;

    public void RegisterEnemy(Health enemyHealth)
    {
        if (enemyHealth != null) 
        {
            remainingEnemies++;
            enemyHealth.OnDeath += HandleEnemyDeath;
            OnEnemyCountChanged?.Invoke();
        }
    }
    private void HandleEnemyDeath()
    {
        remainingEnemies--;
        Debug.Log("Remaining Enemies: " + remainingEnemies);
        OnEnemyCountChanged?.Invoke();
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
