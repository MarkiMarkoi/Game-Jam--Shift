using UnityEngine;

public class ShiftTrigger : MonoBehaviour
{
    private ShiftableRobot shiftableRobot;

    private void Awake()
    {
        shiftableRobot = GetComponentInParent<ShiftableRobot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        BodyShiftController controller = other.GetComponentInParent<BodyShiftController>();
        if (controller != null) 
        {
            controller.SetTarget(shiftableRobot);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        BodyShiftController controller = other.GetComponentInParent<BodyShiftController>();
        if (controller != null)
        {
            controller.ClearTarget(shiftableRobot);
        }
    }
}
