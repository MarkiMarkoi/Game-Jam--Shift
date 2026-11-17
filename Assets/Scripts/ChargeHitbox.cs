using UnityEngine;
using System.Collections.Generic;
public class ChargeHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    private HashSet<Health> hitTargets = new HashSet<Health>();
    private bool isActive = false;
    private PlayerBody ownerBody;

    private void Awake()
    {
        ownerBody = GetComponentInParent<PlayerBody>();
    }

    public void BeginCharge()
    {
        isActive = true;

        hitTargets.Clear();
    }

    public void EndCharge() 
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryHit(other);
    }
    private void OnTriggerStay(Collider other)
    {
        TryHit(other);
    }

    private void TryHit(Collider other)
    {
        Debug.Log("Hitbox berührt : " + other.name);
        if (!isActive) 
        {
            return;
        }
        Debug.Log("Hitbox aktiv, Ziel: " + other.name);
        Health health = other.GetComponentInParent<Health>();
        if(health == null)
        {
            return;
        }
        PlayerBody targetBody = health.GetComponent<PlayerBody>();
        if (targetBody == null || ownerBody == null)
        {
            Debug.LogWarning("PlayerBody fehlt!");
            return;
        }
        Debug.Log($"Owner ist Spieler: {ownerBody.Controller != null}, " + $"Ziel ist Spieler: {targetBody.Controller != null}");
        if (health == ownerBody.GetComponent<Health>())
        {
            return ;
        }

        if((ownerBody.Controller != null) == (targetBody.Controller != null))
        {
            return;
        }
        if (!hitTargets.Add(health))
        {
            return;
        }
        health.TakeDamage(damage);
    }
}
