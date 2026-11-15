using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private float currentHealth;
    private bool isDead;

    public event Action OnDeath;
    public event Action OnHealthChanged;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) 
        {
            return;
        }
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        OnHealthChanged?.Invoke();
        //Für 0 erstmal destroyen
        if (currentHealth == 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }        
    }

    public void Revive(float healthPercentage)
    {
        float percentage = Mathf.Clamp01(healthPercentage);

        currentHealth = maxHealth * percentage;
        OnHealthChanged?.Invoke();
        isDead = false;
    }
}
