using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private WeaponController weapon;
    [SerializeField] private float eyeHeight = 1.0f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private LayerMask obstacleMask;

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

        float distance = Vector3.Distance(transform.position, player.position);

        bool canSeePlayer = CanSeePlayer();

        if (distance > attackRange || !CanSeePlayer()) 
        { 
            agent.isStopped = false;
            agent.SetDestination(player.position);
            LookAtMovement();
        }
        else
        {
            agent.isStopped = true;
            LookAtPlayer();

            if (weapon != null) 
            { 
                weapon.Shoot();
            }
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
}
