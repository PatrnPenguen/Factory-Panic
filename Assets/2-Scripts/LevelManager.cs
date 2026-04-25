using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Factory Settings")]
    public int maxHealth = 5;
    public int currentHealth;
    
    [Header("Win Condition")]
    public int targetFixedMachines = 10;
    public int currentFixedMachines = 0;
    
    private bool isGameOver = false;
    private bool hasWon = false;
    
    public System.Action<int, int> OnHealthChanged;
    public System.Action OnGameOver;
    
    public System.Action<int, int> OnProgressChanged;
    public System.Action OnLevelWon;
    
    
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
        currentFixedMachines = 0;
    }

    void Start()
    {
        NotifyHealthChanged();
        NotifyProgressChanged();
    }
    
    public void DamageFactory(int amount)
    {
        if (isGameOver || hasWon) return;

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

    public void RegisterSuccessfulRepair()
    {
        if (hasWon || isGameOver) return;

        currentFixedMachines++;
        currentFixedMachines = Mathf.Min(currentFixedMachines, targetFixedMachines);

        Debug.Log("Successful repairs: " + currentFixedMachines + " / " + targetFixedMachines);

        NotifyProgressChanged();

        if (currentFixedMachines >= targetFixedMachines)
        {
            WinLevel();
        }
    }

    private void NotifyProgressChanged()
    {
        OnProgressChanged?.Invoke(currentFixedMachines, targetFixedMachines);
    }

    private void WinLevel()
    {
        if (hasWon) return;

        hasWon = true;
        Debug.Log("Level Won!");

        MachineManager machineManager = FindAnyObjectByType<MachineManager>();
        if (machineManager != null)
        {
            machineManager.StopAllMachines();
        }

        OnLevelWon?.Invoke();
    }

    public bool IsLevelComplete()
    {
        return hasWon;
    }
}
