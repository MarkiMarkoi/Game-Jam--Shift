using System.Collections.Generic;
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
                int spawnCount = Mathf.Min(stageData.enemyCount, stageroom.EnemySpawnPoints.Count);
                int guranteedCount = Mathf.Min(spawnCount, stageData.guarenteedEnemyPrefab.Count);
                int randomCount = spawnCount - guranteedCount;

                if (randomCount > 0 && stageData.enemyPrefabs.Count == 0) 
                {
                    Debug.LogWarning("Random enemies needed, but no prefabs assigned");
                    return;
                }
                List<Transform> availablePoints = new List<Transform>(stageroom.EnemySpawnPoints);

                for (int i = 0; i < spawnCount; i++)
                {
                    int selectedPoint = Random.Range(0, availablePoints.Count);
                    Transform point = availablePoints[selectedPoint];
                    GameObject selectedPrefab;
                    if (i < guranteedCount)
                    {
                        selectedPrefab = stageData.guarenteedEnemyPrefab[i];
                    }
                    else
                    {
                        int selectedEnemy = Random.Range(0, stageData.enemyPrefabs.Count);
                        selectedPrefab = stageData.enemyPrefabs[selectedEnemy];
                    }
                    GameObject enemy = Instantiate(selectedPrefab, point.position, point.rotation, currentRoom.transform);
                    Health enemyHealth = enemy.GetComponent<Health>();
                    stageroom.RegisterEnemy(enemyHealth);
                    availablePoints.RemoveAt(selectedPoint);
                }
        }
            stageroom.FinishSpawning();
    }
    

    public void NextStage()
    {
        if (currentRoom != null && stages.Count - 1 > stage) 
        {
            bodyShiftController.CurrentPlayer.SetParent(null);

            Destroy(currentRoom);
            stage++;
            LoadStage();
        }
    }
}
