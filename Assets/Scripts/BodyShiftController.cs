using UnityEngine;
using UnityEngine.InputSystem;

public class BodyShiftController : MonoBehaviour
{
    private ShiftableRobot targetRobot;

    [SerializeField] private InputActionReference shiftInput;
    [SerializeField] private GameObject currentRobot;

    private void Awake()
    {
        currentRobot.GetComponent<PlayerBody>().SetController(this);
    }
    private void OnEnable()
    {
        shiftInput.action.Enable();
    }
    private void OnDisable()
    {
        shiftInput.action.Disable();
    }
    private void Update()
    {
        if(targetRobot == null)
        {
            return;
        }
        if (targetRobot.IsAvailableForShift)
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
        if (targetRobot == null || !targetRobot.IsAvailableForShift)
        {
            return;
        }

        currentRobot.GetComponent<Movement>().enabled = false;
        currentRobot.GetComponent<PlayerCombat>().enabled = false;
        currentRobot.GetComponent<CharacterController>().enabled = false;

        targetRobot.TakeControl();

        GameObject newRobot = targetRobot.gameObject;

        currentRobot.GetComponent<PlayerBody>().SetController(null);
        newRobot.GetComponent<PlayerBody>().SetController(this);

        currentRobot = newRobot;

        targetRobot = null;

        Debug.Log("Body shift completed");
    }
}
