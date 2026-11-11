
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private bool useDistanceDamage = false;
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private float maxDamageMultiplier = 2.5f;

    private float timeLiving;
    private GameObject owner;
    private bool ownerIsPlayer;
    private Vector3 startPosition;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;

        PlayerBody ownerBody = owner.GetComponent<PlayerBody>();
        ownerIsPlayer = ownerBody != null && ownerBody.Controller != null;
    }

    private void Start()
    {
        timeLiving = Time.time + lifeTime;
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (Time.time > timeLiving)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Das Projektil ignoriert seinen eigenen Schützen.
        if (owner != null && other.transform.IsChildOf(owner.transform))
        {
            return;
        }

        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            PlayerBody targetBody = health.GetComponent<PlayerBody>();

            if (targetBody != null)
            {
                bool targetIsPlayer = targetBody.Controller != null;

                // Schaden nur zwischen unterschiedlichen Teams.
                if (ownerIsPlayer != targetIsPlayer)
                {
                    float distance = Vector3.Distance(
                        startPosition,
                        transform.position
                    );

                    float finalDamage = damage;

                    if (useDistanceDamage)
                    {
                        float progress = Mathf.Clamp01(distance / maxDistance);

                        float damageMultiplier = Mathf.Lerp(
                            1f,
                            maxDamageMultiplier,
                            progress
                        );

                        finalDamage *= damageMultiplier;
                    }

                    health.TakeDamage(finalDamage);
                }
            }
        }

        Destroy(gameObject);
    }
}
