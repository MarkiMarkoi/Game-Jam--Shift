using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private Health health;
    private Transform cameraTransform;
    private PlayerBody playerBody;
    private BodyShiftController bodyShiftController;
    private void Start()
    {
        health = GetComponentInParent<Health>();
        health.OnHealthChanged += UpdateHealthUI;
        cameraTransform = Camera.main.transform;
        playerBody = GetComponentInParent<PlayerBody>();
        bodyShiftController = FindFirstObjectByType<BodyShiftController>();
        bodyShiftController.OnBodyChanged += HandleBodyChanged;
    }
    
    private void UpdateHealthUI()
    {
        healthSlider.value = health.CurrentHealth / health.MaxHealth;
    }

    private void OnDestroy()
    {
        health.OnHealthChanged -= UpdateHealthUI;
        bodyShiftController.OnBodyChanged -= HandleBodyChanged;
    }
    private void LateUpdate()
    {
        transform.rotation = cameraTransform.rotation;
    }
    private void HandleBodyChanged()
    {
        if (playerBody != null && playerBody.Controller != null)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
}
