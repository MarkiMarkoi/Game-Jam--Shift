using UnityEngine;
using UnityEngine.InputSystem;

public class BodyShiftController : MonoBehaviour
{
    private ShiftableRobot targetRobot;
    [SerializeField] private InputActionReference shiftInput;

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
                Debug.Log("Shifting...");
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
}
