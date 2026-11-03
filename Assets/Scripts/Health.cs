using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Debug.Log("Current Health: " + currentHealth);
        //Für 0 erstmal destroyen
        if (currentHealth == 0)
        {
            Destroy(gameObject);
        }        
    }
}
