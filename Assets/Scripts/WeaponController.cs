using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireCooldown = 4f;

    private float nextFireTime;
    public void Shoot()
    {
        if(Time.time < nextFireTime)
        {
            return;
        }

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        nextFireTime = Time.time + fireCooldown;
    }

}
