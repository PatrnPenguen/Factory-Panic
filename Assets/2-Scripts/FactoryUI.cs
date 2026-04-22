using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactoryUI : MonoBehaviour
{
    [Header("Health UI")]
    public TMP_Text healthText;
    public Slider healthSlider;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    void Start()
    {
        if (FactoryManager.Instance != null)
        {
            FactoryManager.Instance.OnHealthChanged += UpdateHealthUI;
            FactoryManager.Instance.OnGameOver += ShowGameOver;
            UpdateHealthUI(FactoryManager.Instance.currentHealth, FactoryManager.Instance.maxHealth);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (FactoryManager.Instance != null)
        {
            FactoryManager.Instance.OnHealthChanged -= UpdateHealthUI;
            FactoryManager.Instance.OnGameOver -= ShowGameOver;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + " / " + maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER";
        }
    }
}