using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactoryUI : MonoBehaviour
{
    [Header("Health UI")]
    public TMP_Text healthText;
    public Slider healthSlider;

    [Header("Progress UI")]
    public TMP_Text progressText;
    public Slider progressSlider;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("Win UI")]
    public GameObject winPanel;
    public TMP_Text winText;

    void Start()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnHealthChanged += UpdateHealthUI;
            LevelManager.Instance.OnGameOver += ShowGameOver;
            LevelManager.Instance.OnProgressChanged += UpdateProgressUI;
            LevelManager.Instance.OnLevelWon += ShowWinPanel;

            UpdateHealthUI(LevelManager.Instance.currentHealth, LevelManager.Instance.maxHealth);
            UpdateProgressUI(LevelManager.Instance.currentFixedMachines, LevelManager.Instance.targetFixedMachines);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnHealthChanged -= UpdateHealthUI;
            LevelManager.Instance.OnGameOver -= ShowGameOver;
            LevelManager.Instance.OnProgressChanged -= UpdateProgressUI;
            LevelManager.Instance.OnLevelWon -= ShowWinPanel;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Factory Health: " + currentHealth + " / " + maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void UpdateProgressUI(int currentFixed, int targetFixed)
    {
        if (progressText != null)
        {
            progressText.text = "Machines Fixed: " + currentFixed + " / " + targetFixed;
        }

        if (progressSlider != null)
        {
            progressSlider.maxValue = targetFixed;
            progressSlider.value = currentFixed;
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

    private void ShowWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (winText != null)
        {
            winText.text = "LEVEL COMPLETE";
        }
    }

    public void OnRetryButtonClicked()
    {
        if (LevelSceneManager.Instance != null)
        {
            LevelSceneManager.Instance.RetryCurrentLevel();
        }
    }

    public void OnMainMenuButtonClicked()
    {
        if (LevelSceneManager.Instance != null)
        {
            LevelSceneManager.Instance.LoadMainMenu();
        }
    }

    public void OnNextLevelButtonClicked()
    {
        if (LevelSceneManager.Instance != null)
        {
            LevelSceneManager.Instance.LoadNextLevel();
        }
    }
}