using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShiftableRobot : MonoBehaviour
{
    private Health health;
    private NavMeshAgent navMeshAgent;
    private bool isAvailableForShift;

    public PlayerBody playerBody;

    [SerializeField] private float takeoverWindow = 5f;
    [SerializeField] private Behaviour enemyBehaviour;
    public bool IsAvailableForShift => isAvailableForShift;
    private void Awake()
    {
        playerBody = GetComponent<PlayerBody>();
        health = GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        health.OnDeath += EnterDyingState;
    }
    private void OnDisable()
    {
        health.OnDeath -= EnterDyingState;
    }

    private void EnterDyingState()
    {
        if (playerBody != null && playerBody.Controller != null) 
        {
            return;
        }
        navMeshAgent.isStopped = true;
        if (enemyBehaviour != null) 
        {
            enemyBehaviour.enabled = false;
        }
        navMeshAgent.enabled = false;
        isAvailableForShift = true;
        StartCoroutine(DespawnAfterDelay());
    }

    public void MarkAsTaken()
    {
        isAvailableForShift = false;
    }

    IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(takeoverWindow);
        if (isAvailableForShift)
        {
            Destroy(gameObject);
        }
    }

    public void TakeControl()
    {
        isAvailableForShift = false;

        navMeshAgent.enabled = false;
        if (enemyBehaviour != null)
        {
            enemyBehaviour.enabled = false;
        }
        health.Revive(0.5f);
        GetComponent<CharacterController>().enabled = true;
        GetComponent <Movement>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
    }

}
