using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public BodyShiftController Controller {  get; private set; }

    public void SetController(BodyShiftController controller)
    {
        Controller = controller;
    }
}
