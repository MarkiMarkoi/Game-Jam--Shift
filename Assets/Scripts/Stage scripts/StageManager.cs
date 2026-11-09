using UnityEngine;
using System.Collections.Generic;
public class StageManager : MonoBehaviour
{
    [SerializeField] private int stage = 0;
    [SerializeField] private List<GameObject> rooms;
    private GameObject currentRoom;
    private BodyShiftController bodyShiftController;
    [SerializeField] private GameObject enemyPrefab;
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
        GameObject room = Instantiate(rooms[stage], Vector3.zero, Quaternion.identity);
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
            foreach(Transform point in stageroom.EnemySpawnPoints)
            {
                GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation, currentRoom.transform);
                Health enemyHealth = enemy.GetComponent<Health>();
                stageroom.RegisterEnemy(enemyHealth);
            }
        }
    }

    public void NextStage()
    {
        if (currentRoom != null && rooms.Count - 1 > stage) 
        {
            Destroy(currentRoom);
            stage++;
            LoadStage();
        }
    }
}
