using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime = 3f;

    private float timeLiving;

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
        Destroy(gameObject);
    }
}
