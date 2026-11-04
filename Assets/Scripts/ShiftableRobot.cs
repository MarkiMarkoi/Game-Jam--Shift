using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShiftableRobot : MonoBehaviour
{
    private Health health;
    private EnemyAI enemyAI;
    private NavMeshAgent navMeshAgent;
    private bool isAvailableForShift;

    [SerializeField] private float takeoverWindow = 5f;
    public bool IsAvailableForShift => isAvailableForShift;
    private void Awake()
    {
        health = GetComponent<Health>();
        enemyAI = GetComponent<EnemyAI>();
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
        navMeshAgent.isStopped = true;
        enemyAI.enabled = false;
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
}
