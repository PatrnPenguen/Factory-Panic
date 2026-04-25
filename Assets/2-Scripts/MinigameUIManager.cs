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

        int pressCount = 7;
        float timeLimit = 5f;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                pressCount = 7;
                timeLimit = 5f;
                break;

            case MinigameDifficulty.Medium:
                pressCount = 14;
                timeLimit = 5f;
                break;

            case MinigameDifficulty.Hard:
                pressCount = 21;
                timeLimit = 4f;
                break;
        }

        tapRepairPanel.Configure(pressCount, timeLimit, difficulty);
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
        int stageCount = 1;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                moveSpeed = 1.2f;
                successMin = 0.32f;
                successMax = 0.68f;
                timeLimit = 5f;
                stageCount = 1;
                break;

            case MinigameDifficulty.Medium:
                moveSpeed = 1.5f;
                successMin = 0.36f;
                successMax = 0.64f;
                timeLimit = 4.5f;
                stageCount = 2;
                break;

            case MinigameDifficulty.Hard:
                moveSpeed = 1.9f;
                successMin = 0.40f;
                successMax = 0.60f;
                timeLimit = 5f;
                stageCount = 3;
                break;
        }

        timingRepairPanel.Configure(moveSpeed, successMin, successMax, timeLimit, stageCount);
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