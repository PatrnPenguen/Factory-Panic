using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    public static FactoryManager Instance;

    [Header("Factory Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    private bool isGameOver = false;

    public System.Action<int, int> OnHealthChanged;
    public System.Action OnGameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentHealth = maxHealth;
    }

    void Start()
    {
        NotifyHealthChanged();
    }

    public void DamageFactory(int amount)
    {
        if (isGameOver) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Factory damaged. Current health: " + currentHealth);

        NotifyHealthChanged();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        OnGameOver?.Invoke();
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}