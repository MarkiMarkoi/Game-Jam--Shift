using UnityEngine;

public class BodyShiftController : MonoBehaviour
{
    private ShiftableRobot targetRobot;

    public void SetTarget(ShiftableRobot robot)
    {
        targetRobot = robot;
    }

    public void ClearTarget(ShiftableRobot robot)
    {
        if (targetRobot == robot)
        {
            targetRobot = null;

        }
    }
}
