using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BodyShiftController : MonoBehaviour
{
    private ShiftableRobot targetRobot;
    private ShiftTransition transition;
    [SerializeField] private InputActionReference shiftInput;
    [SerializeField] private GameObject currentRobot;
    [SerializeField] private CinemachineCamera playerCamera;

    public Transform CurrentPlayer => currentRobot.transform;

    private bool isShifting;
    private ShiftableRobot pendingRobot;
    private GameManager gameManager;
    private Health playerHealth;
    private void Awake()
    {
        currentRobot.GetComponent<PlayerBody>().SetController(this);
        playerHealth = currentRobot.GetComponent<Health>();
        transition = GetComponent<ShiftTransition>();
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnEnable()
    {
        shiftInput.action.Enable();
        transition.OnTransitionComplete += CompleteBodyShift;
        playerHealth.OnDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        shiftInput.action.Disable();
        transition.OnTransitionComplete -= CompleteBodyShift;
        playerHealth.OnDeath -= HandlePlayerDeath;
    }
    private void Update()
    {
        if(targetRobot == null)
        {
            return;
        }
        if (targetRobot.IsAvailableForShift && !isShifting)
        {
            Debug.Log("Press E to Shift");

            if (shiftInput.action.WasPressedThisFrame())
            {
                SwitchBody();
            }
        }
    }
    public void SetTarget(ShiftableRobot robot)
    {
        targetRobot = robot;
        Debug.Log("Shift target found");
    }

    public void ClearTarget(ShiftableRobot robot)
    {
        if (targetRobot == robot)
        {
            targetRobot = null;
            Debug.Log("Shift target lost");
        }
    }

    private void SwitchBody()
    {
        if (targetRobot == null || !targetRobot.IsAvailableForShift || isShifting == true)
        {
            return;
        }
        Time.timeScale = 0f;
        isShifting = true;
        targetRobot.MarkAsTaken();
        pendingRobot = targetRobot;
        GameObject oldRobot = currentRobot;

        currentRobot.GetComponent<Movement>().enabled = false;
        currentRobot.GetComponent<PlayerCombat>().enabled = false;
        currentRobot.GetComponent<CharacterController>().enabled = false;

        GameObject newRobot = targetRobot.gameObject;
        GameObject lightSphere = transition.StartTransition(oldRobot.transform.position, newRobot.transform.position);
        playerCamera.Follow = lightSphere.transform;
        Debug.Log("1. SwitchBody wurde aufgerufen");
    }

    private void CompleteBodyShift()
    {
        playerHealth.OnDeath -= HandlePlayerDeath;

        currentRobot.GetComponent<PlayerBody>().SetController(null);
        pendingRobot.GetComponent<PlayerBody>().SetController(this);

        playerHealth = pendingRobot.GetComponent<Health>();
        playerHealth.OnDeath += HandlePlayerDeath;

        playerCamera.Follow = pendingRobot.transform;

        Destroy(currentRobot);

        currentRobot = pendingRobot.gameObject;
        pendingRobot.TakeControl();

        targetRobot = null;
        pendingRobot = null;
        isShifting = false;

        Time.timeScale = 1f;

        Debug.Log("Transition complete");
    }

    private void HandlePlayerDeath()
    {
        Movement playerMovement = currentRobot.GetComponent<Movement>();
        if (playerMovement != null) 
        { 
            playerMovement.enabled = false;
        }
        gameManager.GameOver();
    }
}
