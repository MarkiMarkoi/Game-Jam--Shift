using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float minimumDistance = 3f;
    [SerializeField] private float retreatDistance = 4f;

    [SerializeField] private WeaponController weapon;

    [SerializeField] private float eyeHeight = 1.0f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private bool canRetreat = false;

    private bool isRetreating = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update()
    {
        if(player == null || !agent.isOnNavMesh)
        {
            return;
        }
        if (isRetreating)
        {
            UpdateRetreat();
            return;
        }
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange || !CanSeePlayer()) 
        { 
            agent.isStopped = false;
            agent.SetDestination(player.position);
            LookAtMovement();
        }
        else if(minimumDistance > distance && canRetreat)
        {
            SeekDistance();
        }
        else
        {
            AttackPlayer();
        }
    }

    private void UpdateRetreat()
    {
        if (agent.pathPending)
        {
            return;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid ||
            agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            isRetreating = false;
            AttackPlayer();
            return;
        }

        if (agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            isRetreating = false;
        }

    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool CanSeePlayer() 
    { 
        Vector3 origin = transform.position + Vector3.up * eyeHeight;

        Vector3 target = player.position + Vector3.up * eyeHeight;

        Vector3 direction = target - origin;

        Debug.DrawRay(origin,direction, Color.yellow);

        bool wallHit = Physics.Raycast(origin,direction.normalized, direction.magnitude,obstacleMask);

        return !wallHit;
    }

    private void LookAtMovement()
    {
        Vector3 direction = agent.velocity;
        direction.y = 0f;
        if(direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SeekDistance()
    {
        Vector3 retreatDirection = (transform.position - player.position).normalized;

        Vector3 retreatPosition = transform.position + retreatDirection * retreatDistance;
        if (NavMesh.SamplePosition(retreatPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            float currentDistance = Vector3.Distance(transform.position, player.position);
            float newDistance = Vector3.Distance(player.position, hit.position);

            if (newDistance > currentDistance + 1f)
            {
                agent.isStopped = false;
                if (agent.SetDestination(hit.position))
                {
                    isRetreating = true;
                }
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;
        LookAtPlayer();
        if(weapon != null && CanSeePlayer())
        {
            weapon.Shoot();
        }
    }
}
