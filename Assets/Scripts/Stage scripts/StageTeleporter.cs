using UnityEngine;

public class StageTeleporter : MonoBehaviour
{
    private StageManager stageManager;

    private void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerBody playerBody = other.GetComponentInParent<PlayerBody>();
        if (playerBody != null && playerBody.Controller != null) 
        {
            stageManager.NextStage();
        }
    }
}
