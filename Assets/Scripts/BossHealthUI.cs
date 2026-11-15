using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private TMP_Text bossHealthText;
    [SerializeField] private Slider bossHealthSlider;

    private Health bossHealth;

    public void SetBoss(Boss boss)
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged -= UpdateHealthUI;
            bossHealth.OnDeath -= HandleBossDeath;
        }
        bossNameText.text = boss.BossName;
        bossHealth = boss.GetComponent<Health>();
        gameObject.SetActive(true);
        bossHealth.OnHealthChanged += UpdateHealthUI;
        bossHealth.OnDeath += HandleBossDeath;
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        if (bossHealth != null)
        {
            bossHealthText.text = Mathf.CeilToInt(bossHealth.CurrentHealth) + " / " + Mathf.CeilToInt(bossHealth.MaxHealth);
            bossHealthSlider.value = bossHealth.CurrentHealth / bossHealth.MaxHealth;
        }
    }
    private void HandleBossDeath()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (bossHealth != null) 
        { 
            bossHealth.OnHealthChanged -= UpdateHealthUI;

            bossHealth.OnDeath -= HandleBossDeath;
        }
    }
}
