using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float damage = 1f;
    private float timeLiving;

    private GameObject owner;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
    private void Start()
    {
        timeLiving = Time.time + lifeTime;
    }
    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        if(Time.time > timeLiving)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.transform.IsChildOf(owner.transform)) 
        {
            return;
        }
        Health health = other.GetComponentInParent<Health>();
        if (health != null) 
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
