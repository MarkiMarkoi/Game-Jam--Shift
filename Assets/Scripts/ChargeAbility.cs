using UnityEngine;

public class ChargeAbility : MonoBehaviour
{
    [SerializeField] private float chargeSpeed = 12f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float chargeCoolDown = 2f;
    [SerializeField] private ChargeHitbox chargeHitbox;

    private CharacterController characterController;
    private Movement movement;
    private Vector3 chargeDirection;
    private float chargeEndTime;
    private float nextChargeTime;
    private bool isCharging;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movement = GetComponent<Movement>();
    }

    public void StartCharge()
    {
        if (isCharging || Time.time < nextChargeTime)
        {
            return;
        }
        if (!characterController.enabled)
        {
            return;
        }
        chargeDirection = transform.forward;
        chargeDirection.y = 0f;
        chargeDirection.Normalize();
        chargeEndTime = Time.time + chargeDuration;
        nextChargeTime = Time.time + chargeCoolDown;
        isCharging = true;
        movement.enabled = false;
        chargeHitbox.BeginCharge();
    }
    private void Update()
    {
        if (!isCharging) 
        {
            return;
        }
        if(Time.time >= chargeEndTime)
        {
            isCharging = false;
            chargeHitbox.EndCharge();
            movement.enabled = true;
            return;
        }
        characterController.Move(chargeDirection * chargeSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        isCharging = false;

        if(chargeHitbox != null)
        {
            chargeHitbox.EndCharge();
        }

        if(movement != null)
        {
            movement.enabled = false;
        }
    }
}
