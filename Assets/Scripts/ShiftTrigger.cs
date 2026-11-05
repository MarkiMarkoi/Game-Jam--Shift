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
        PlayerBody playerBody = other.GetComponentInParent<PlayerBody>();

        if (playerBody != null && playerBody.Controller != null)
        {
            playerBody.Controller.SetTarget(shiftableRobot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerBody playerBody = other.GetComponentInParent<PlayerBody>();

        if (playerBody != null && playerBody.Controller != null)
        {
            playerBody.Controller.ClearTarget(shiftableRobot);
        }
    }
}