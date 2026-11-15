using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarFill;
    [SerializeField] private TMP_Text healthText;

    private Health currentHealth;
    private BodyShiftController bodyShiftController;
    private void Start()
    {
        bodyShiftController = FindFirstObjectByType<BodyShiftController>();
        currentHealth = bodyShiftController.CurrentPlayer.GetComponent<Health>();
        currentHealth.OnHealthChanged += UpdateHealthUI;
        bodyShiftController.OnBodyChanged += HandleBodyChanged;
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        healthText.text = Mathf.CeilToInt(currentHealth.CurrentHealth) + " / " + Mathf.CeilToInt(currentHealth.MaxHealth);
        healthBarFill.value = currentHealth.CurrentHealth / currentHealth.MaxHealth;
    }
    private void OnDestroy()
    {
        if(currentHealth != null)
        {
            currentHealth.OnHealthChanged -= UpdateHealthUI;
        }
        if(bodyShiftController != null)
        {
            bodyShiftController.OnBodyChanged -= HandleBodyChanged;
        }
    }

    private void HandleBodyChanged()
    {
        currentHealth.OnHealthChanged -= UpdateHealthUI;
        currentHealth = bodyShiftController.CurrentPlayer.GetComponent<Health>();
        currentHealth.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }
}
