using UnityEngine;
using UnityEngine.AI;

public class ChargerAI : MonoBehaviour
{
    private enum ChargerState
    {
        Chase,
        Windup,
        Charging,
        Recovery
    }

    [SerializeField] private ChargeHitbox chargeHitBox;

    [SerializeField] private float chargeRange = 5f;
    [SerializeField] private float chargeSpeed = 12f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float windupDuration = 1f;
    [SerializeField] private float recoveryDuration = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;

    private float windupEndTime;
    private float chargeEndTime;
    private float recoveryEndTime;

    private ChargerState currentState = ChargerState.Chase;

    private NavMeshAgent agent;
    private BodyShiftController bodyShiftController;
    private Vector3 chargeDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        bodyShiftController = FindFirstObjectByType<BodyShiftController>();

        agent.updateRotation = false;
    }

    private void Update()
    {
        switch (currentState) 
        {
            case ChargerState.Chase:
                // Verfolgungsverhalten
                UpdateChase();
                break;

            case ChargerState.Windup:
                // Angriff vorbereiten
                UpdateWindup();
                break;

            case ChargerState.Charging:
                // Sturmangriff
                UpdateCharging();
                break;

            case ChargerState.Recovery:
                // Erholungsphase
                UpdateRecovery();
                break;
        }

    }

    private void UpdateChase()
    {
        if (agent != null && agent.isOnNavMesh && bodyShiftController != null)
        {
            float distance = Vector3.Distance(transform.position, bodyShiftController.CurrentPlayer.position);
            if (distance > chargeRange) 
            {
                agent.isStopped = false;
                agent.SetDestination(bodyShiftController.CurrentPlayer.position);
                Vector3 lookDirection = bodyShiftController.CurrentPlayer.position - transform.position;
                lookDirection.y = 0f;
                if (lookDirection.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                agent.isStopped = true;
                windupEndTime = Time.time + windupDuration;
                currentState = ChargerState.Windup;
            }
        }
    }
    private void UpdateWindup()
    {
        Vector3 lookDirection = bodyShiftController.CurrentPlayer.position - transform.position;
        lookDirection.y = 0f;
        if(lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if (Time.time >= windupEndTime)
        {
            Debug.Log("Charging!");
            Vector3 direction = bodyShiftController.CurrentPlayer.position - transform.position;
            direction.y = 0f;
            chargeDirection = direction.normalized;
            chargeEndTime = Time.time + chargeDuration;
            chargeHitBox.BeginCharge();
            currentState = ChargerState.Charging;
        }
    }
    private void UpdateCharging()
    {
        if (Time.time >= chargeEndTime)
        {
            currentState = ChargerState.Recovery;
            recoveryEndTime = Time.time + recoveryDuration;
            chargeHitBox.EndCharge();
            return;
        }

        if (chargeDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(chargeDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        agent.Move(chargeDirection * chargeSpeed * Time.deltaTime);
    }
    private void UpdateRecovery()
    {
        if(Time.time >= recoveryEndTime)
        {
            currentState = ChargerState.Chase;
        }
    }
}
