using UnityEngine;

public class MinigameUIManager : MonoBehaviour
{
    public static MinigameUIManager Instance;

    [Header("Panels")]
    public GameObject rootPanel;
    public TapRepairPanel tapRepairPanel;
    public TimingRepairPanel timingRepairPanel;

    private Machine currentMachine;
    private bool isOpen = false;

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

        CloseAllInstant();
    }

    public void OpenMinigame(Machine machine)
    {
        if (machine == null) return;
        if (isOpen) return;

        currentMachine = machine;
        isOpen = true;

        if (rootPanel != null)
        {
            rootPanel.SetActive(true);
        }

        OpenPanelByType(machine.minigameType);
    }

    private void OpenPanelByType(MinigameType minigameType)
    {
        HideAllMinigamePanels();

        if (currentMachine == null)
        {
            ResolveCurrentMinigame(false);
            return;
        }

        switch (minigameType)
        {
            case MinigameType.TapRepair:
                OpenTapRepairByDifficulty(currentMachine.minigameDifficulty);
                break;

            case MinigameType.TimingRepair:
                OpenTimingRepairByDifficulty(currentMachine.minigameDifficulty);
                break;

            default:
                ResolveCurrentMinigame(false);
                break;
        }
    }
    
    private void OpenTapRepairByDifficulty(MinigameDifficulty difficulty)
    {
        if (tapRepairPanel == null) return;

        int pressCount = 8;
        float timeLimit = 4f;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                pressCount = 5;
                timeLimit = 5f;
                break;

            case MinigameDifficulty.Medium:
                pressCount = 8;
                timeLimit = 4f;
                break;

            case MinigameDifficulty.Hard:
                pressCount = 12;
                timeLimit = 3f;
                break;
        }

        tapRepairPanel.Configure(pressCount, timeLimit);
        tapRepairPanel.gameObject.SetActive(true);
        tapRepairPanel.Begin(this);
    }

    private void OpenTimingRepairByDifficulty(MinigameDifficulty difficulty)
    {
        if (timingRepairPanel == null) return;

        float moveSpeed = 1.5f;
        float successMin = 0.4f;
        float successMax = 0.6f;
        float timeLimit = 4f;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                moveSpeed = 1.2f;
                successMin = 0.30f;
                successMax = 0.70f;
                timeLimit = 5f;
                break;

            case MinigameDifficulty.Medium:
                moveSpeed = 1.6f;
                successMin = 0.42f;
                successMax = 0.58f;
                timeLimit = 4f;
                break;

            case MinigameDifficulty.Hard:
                moveSpeed = 2.2f;
                successMin = 0.47f;
                successMax = 0.53f;
                timeLimit = 3f;
                break;
        }

        timingRepairPanel.Configure(moveSpeed, successMin, successMax, timeLimit);
        timingRepairPanel.gameObject.SetActive(true);
        timingRepairPanel.Begin(this);
    }

    public void ResolveCurrentMinigame(bool success)
    {
        if (currentMachine != null)
        {
            currentMachine.CompleteMinigame(success);
        }

        currentMachine = null;
        isOpen = false;
        CloseAllInstant();
    }

    public void CancelCurrentMinigame()
    {
        if (currentMachine != null)
        {
            currentMachine.CompleteMinigame(false);
        }

        currentMachine = null;
        isOpen = false;
        CloseAllInstant();
    }

    public bool IsMinigameOpen()
    {
        return isOpen;
    }

    private void HideAllMinigamePanels()
    {
        if (tapRepairPanel != null)
        {
            tapRepairPanel.gameObject.SetActive(false);
        }

        if (timingRepairPanel != null)
        {
            timingRepairPanel.gameObject.SetActive(false);
        }
    }

    private void CloseAllInstant()
    {
        if (rootPanel != null)
        {
            rootPanel.SetActive(false);
        }

        HideAllMinigamePanels();
    }
}