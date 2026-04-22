using UnityEngine;

public class Machine : MonoBehaviour
{
    [Header("Machine Identity")]
    public string machineId = "Machine_01";

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

    private Renderer objectRenderer;
    private Color normalColor = Color.white;
    private Color brokenColor = Color.red;
    private Color cooldownColor = Color.gray;
    private Color minigameColor = Color.yellow;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            normalColor = objectRenderer.material.color;
        }

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
        Debug.Log(machineId + " broke down.");
    }

    public void StartMinigame()
    {
        if (currentState != MachineState.Broken) return;

        currentState = MachineState.InMinigame;
        UpdateVisual();

        Debug.Log(machineId + " minigame started.");

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
            Debug.Log(machineId + " repaired successfully.");
            StartCooldown();
        }
        else
        {
            Debug.Log(machineId + " minigame failed.");
            FactoryManager.Instance.DamageFactory(damageOnMinigameFail);
            StartCooldown();
        }
    }

    private void HandleBrokenTimeout()
    {
        if (currentState != MachineState.Broken) return;

        Debug.Log(machineId + " timeout. Factory takes damage.");
        FactoryManager.Instance.DamageFactory(damageOnTimeout);
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

        Debug.Log(machineId + " is ready again.");
    }

    private void UpdateVisual()
    {
        if (objectRenderer == null) return;

        switch (currentState)
        {
            case MachineState.Idle:
                objectRenderer.material.color = normalColor;
                break;
            case MachineState.Broken:
                objectRenderer.material.color = brokenColor;
                break;
            case MachineState.InMinigame:
                objectRenderer.material.color = minigameColor;
                break;
            case MachineState.Cooldown:
                objectRenderer.material.color = cooldownColor;
                break;
        }
    }

    public float GetBrokenTimeNormalized()
    {
        if (currentState != MachineState.Broken) return 0f;
        if (brokenDuration <= 0f) return 0f;

        return Mathf.Clamp01(currentBrokenTimer / brokenDuration);
    }
}