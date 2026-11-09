using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Timeline.Actions;
using UnityEngine;
public class StageManager : MonoBehaviour
{
    [SerializeField] private int stage = 0;
    [SerializeField] private List<StageData> stages;
    private GameObject currentRoom;
    private BodyShiftController bodyShiftController;
    private void Awake()
    {
        bodyShiftController = FindFirstObjectByType<BodyShiftController>();
    }
    void Start()
    {
        LoadStage();
    }

    private void LoadStage()
    {
        StageData stageData = stages[stage];
        GameObject room = Instantiate(stageData.roomPrefab, Vector3.zero, Quaternion.identity);
        currentRoom = room;
        StageRoom stageroom = currentRoom.GetComponent<StageRoom>();
        if (stageroom != null)
        {
            Transform playerSpawn = stageroom.PlayerSpawn;
            Transform player = bodyShiftController.CurrentPlayer;
            CharacterController playerCc = player.GetComponent<CharacterController>();

            if (playerCc != null)
            {
                playerCc.enabled = false;

                player.position = playerSpawn.position;
                player.rotation = playerSpawn.rotation;

                playerCc.enabled = true;
            }
            if(stageData.enemyPrefabs.Count > 0 && stageData.enemyCount > 0)
            {
                int spawnCount = Mathf.Min(stageData.enemyCount, stageroom.EnemySpawnPoints.Count);

                List<Transform> availablePoints = new List<Transform>(stageroom.EnemySpawnPoints);

                for (int i = 0; i < spawnCount; i++)
                {
                    int selectedPoint = Random.Range(0, availablePoints.Count);
                    Transform point = availablePoints[selectedPoint];
                    int selectedEnemy = Random.Range(0, stageData.enemyPrefabs.Count);
                    GameObject enemy = Instantiate(stageData.enemyPrefabs[selectedEnemy], point.position, point.rotation, currentRoom.transform);
                    Health enemyHealth = enemy.GetComponent<Health>();
                    stageroom.RegisterEnemy(enemyHealth);
                    availablePoints.RemoveAt(selectedPoint);
                }
            }
            stageroom.FinishSpawning();
        }
    }

    public void NextStage()
    {
        if (currentRoom != null && stages.Count - 1 > stage) 
        {
            Destroy(currentRoom);
            stage++;
            LoadStage();
        }
    }
}
