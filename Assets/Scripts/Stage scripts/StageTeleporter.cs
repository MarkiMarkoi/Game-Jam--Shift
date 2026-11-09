using UnityEngine;

public class StageTeleporter : MonoBehaviour
{
    private StageManager stageManager;
    private bool isUnlocked = false;
    private bool hasTeleported = false;
    private void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        TryTeleporting(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryTeleporting(other);
    }

    public void Unlock()
    {
        isUnlocked = true;
    }

    private void TryTeleporting(Collider body)
    {
        PlayerBody playerBody = body.GetComponentInParent<PlayerBody>();
        if (playerBody != null && playerBody.Controller != null)
        {
            if (isUnlocked && !hasTeleported)
            {
                hasTeleported = true;
                stageManager.NextStage();
            }
        }
    }
}
