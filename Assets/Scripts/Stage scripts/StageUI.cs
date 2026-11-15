using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text enemyCountText;
    private StageManager stageManager;
    private StageRoom currentStageRoom;
    private void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();
    }

    private void Start()
    {
        stageManager.OnStageChanged += HandleStageChanged;
        HandleStageChanged();
    }

    private void UpdateStageUI()
    {
        if (stageManager != null)
        {
            stageText.text = "STAGE " + stageManager.CurrentStage;
        }
    }

    private void UpdateEnemyCountUI()
    {
        if (currentStageRoom != null) 
        { 
            enemyCountText.text = "ENEMIES: " + currentStageRoom.RemainingEnemies;
        }
    }
    private void OnDestroy()
    {
        if (stageManager != null)
        {
            stageManager.OnStageChanged -= HandleStageChanged;
        }
        if(currentStageRoom != null)
        {
            currentStageRoom.OnEnemyCountChanged -= UpdateEnemyCountUI;
        }
    }
    private void HandleStageChanged()
    {
        if(currentStageRoom  != null)
        {
            currentStageRoom.OnEnemyCountChanged -= UpdateEnemyCountUI;
        }
        currentStageRoom = stageManager.CurrentRoom;

        if(currentStageRoom != null)
        {
            currentStageRoom.OnEnemyCountChanged += UpdateEnemyCountUI;
        }
        UpdateStageUI();
        UpdateEnemyCountUI();
    }
}
