
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireCooldown = 4f;

    private float nextFireTime;

    public void Shoot()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        GameObject newProjectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Projectile projectile = newProjectile.GetComponent<Projectile>();

        if (projectile != null)
        {
            PlayerBody ownerBody = GetComponentInParent<PlayerBody>();

            if (ownerBody != null)
            {
                projectile.SetOwner(ownerBody.gameObject);
            }
            else
            {
                Debug.LogWarning("WeaponController: No PlayerBody found!", this);
            }
        }

        nextFireTime = Time.time + fireCooldown;
    }
}
