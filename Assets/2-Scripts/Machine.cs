using UnityEngine;

public class Machine : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject healthyVisual;
    public GameObject brokenVisual;

    [Header("Timing")]
    public float brokenDuration = 8f;
    public float rebreakCooldown = 10f;

    [Header("Penalty")]
    public int damageOnTimeout = 1;
    public int damageOnMinigameFail = 1;

    [Header("Debug")]
    public MachineState currentState = MachineState.Idle;
    public float currentBrokenTimer = 0f;
    public float currentCooldownTimer = 0f;

    [Header("Minigame")]
    public MinigameType minigameType = MinigameType.TapRepair;
    public MinigameDifficulty minigameDifficulty = MinigameDifficulty.Easy;

    void Awake()
    {
        UpdateVisual();
    }

    void Update()
    {
        UpdateBrokenState();
        UpdateCooldownState();
    }

    private void UpdateBrokenState()
    {
        if (currentState != MachineState.Broken) return;

        currentBrokenTimer -= Time.deltaTime;

        if (currentBrokenTimer <= 0f)
        {
            HandleBrokenTimeout();
        }
    }

    private void UpdateCooldownState()
    {
        if (currentState != MachineState.Cooldown) return;

        currentCooldownTimer -= Time.deltaTime;

        if (currentCooldownTimer <= 0f)
        {
            SetIdle();
        }
    }

    public bool CanBreak()
    {
        return currentState == MachineState.Idle;
    }

    public bool CanInteract()
    {
        return currentState == MachineState.Broken;
    }

    public void BreakMachine()
    {
        if (!CanBreak()) return;

        currentState = MachineState.Broken;
        currentBrokenTimer = brokenDuration;

        UpdateVisual();

    }

    public void StartMinigame()
    {
        if (currentState != MachineState.Broken) return;

        currentState = MachineState.InMinigame;

        UpdateVisual();

        if (MinigameUIManager.Instance != null)
        {
            MinigameUIManager.Instance.OpenMinigame(this);
        }
    }

    public void CompleteMinigame(bool success)
    {
        if (currentState != MachineState.InMinigame) return;

        if (success)
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.RegisterSuccessfulRepair();
            }

            StartCooldown();
        }
        else
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.DamageFactory(damageOnMinigameFail);
            }

            StartCooldown();
        }
    }

    private void HandleBrokenTimeout()
    {
        if (currentState != MachineState.Broken) return;
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.DamageFactory(damageOnTimeout);
        }

        StartCooldown();
    }

    private void StartCooldown()
    {
        currentState = MachineState.Cooldown;
        currentCooldownTimer = rebreakCooldown;

        UpdateVisual();
    }

    private void SetIdle()
    {
        currentState = MachineState.Idle;
        currentBrokenTimer = 0f;
        currentCooldownTimer = 0f;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bool shouldShowBroken = currentState == MachineState.Broken || currentState == MachineState.InMinigame;
        bool shouldShowHealthy = !shouldShowBroken;

        if (healthyVisual != null)
        {
            healthyVisual.SetActive(shouldShowHealthy);
        }

        if (brokenVisual != null)
        {
            brokenVisual.SetActive(shouldShowBroken);
        }
    }

    public float GetBrokenTimeNormalized()
    {
        if (currentState != MachineState.Broken) return 0f;
        if (brokenDuration <= 0f) return 0f;

        return Mathf.Clamp01(currentBrokenTimer / brokenDuration);
    }

    public void ForceStopMachine()
    {
        currentState = MachineState.Idle;
        currentBrokenTimer = 0f;
        currentCooldownTimer = 0f;

        UpdateVisual();
    }
}