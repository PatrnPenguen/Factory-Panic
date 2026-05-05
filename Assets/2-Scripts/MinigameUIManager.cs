using UnityEngine;

public class MinigameUIManager : MonoBehaviour
{
    public static MinigameUIManager Instance;

    [Header("Panels")]
    public GameObject rootPanel;
    public TapRepairPanel tapRepairPanel;
    public TimingRepairPanel timingRepairPanel;
    public SequenceRepairPanel sequenceRepairPanel;
    public HoldRepairPanel holdRepairPanel;

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

            case MinigameType.SequenceRepair:
                OpenSequenceRepairByDifficulty(currentMachine.minigameDifficulty);
                break;

            case MinigameType.HoldRepair:
                OpenHoldRepairByDifficulty(currentMachine.minigameDifficulty);
                break;

            default:
                ResolveCurrentMinigame(false);
                break;
        }
    }

    private void OpenTapRepairByDifficulty(MinigameDifficulty difficulty)
    {
        if (tapRepairPanel == null) return;

        int pressCount = 5;
        float timeLimit = 5f;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                pressCount = 5;
                timeLimit = 7f;
                break;

            case MinigameDifficulty.Medium:
                pressCount = 9;
                timeLimit = 6f;
                break;

            case MinigameDifficulty.Hard:
                pressCount = 13;
                timeLimit = 7f;
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
                successMin = 0.30f;
                successMax = 0.70f;
                timeLimit = 7f;
                stageCount = 1;
                break;

            case MinigameDifficulty.Medium:
                moveSpeed = 1.5f;
                successMin = 0.35f;
                successMax = 0.65f;
                timeLimit = 6f;
                stageCount = 2;
                break;

            case MinigameDifficulty.Hard:
                moveSpeed = 1.7f;
                successMin = 0.38f;
                successMax = 0.62f;
                timeLimit = 7f;
                stageCount = 3;
                break;
        }

        timingRepairPanel.Configure(moveSpeed, successMin, successMax, timeLimit, stageCount);
        timingRepairPanel.gameObject.SetActive(true);
        timingRepairPanel.Begin(this);
    }

    private void OpenSequenceRepairByDifficulty(MinigameDifficulty difficulty)
    {
        if (sequenceRepairPanel == null) return;

        int sequenceLength = 3;
        float timeLimit = 6f;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                sequenceLength = 3;
                timeLimit = 7f;
                break;

            case MinigameDifficulty.Medium:
                sequenceLength = 5;
                timeLimit = 6f;
                break;

            case MinigameDifficulty.Hard:
                sequenceLength = 7;
                timeLimit = 7f;
                break;
        }

        sequenceRepairPanel.Configure(sequenceLength, timeLimit);
        sequenceRepairPanel.gameObject.SetActive(true);
        sequenceRepairPanel.Begin(this);
    }

    private void OpenHoldRepairByDifficulty(MinigameDifficulty difficulty)
    {
        if (holdRepairPanel == null) return;

        float fillSpeed = 0.65f;
        float drainSpeed = 0.35f;
        float successMin = 0.42f;
        float successMax = 0.68f;
        float timeLimit = 5f;
        int stageCount = 1;

        switch (difficulty)
        {
            case MinigameDifficulty.Easy:
                fillSpeed = 0.65f;
                drainSpeed = 0.25f;
                successMin = 0.36f;
                successMax = 0.76f;
                timeLimit = 7f;
                stageCount = 1;
                break;

            case MinigameDifficulty.Medium:
                fillSpeed = 0.75f;
                drainSpeed = 0.35f;
                successMin = 0.40f;
                successMax = 0.70f;
                timeLimit = 6f;
                stageCount = 2;
                break;

            case MinigameDifficulty.Hard:
                fillSpeed = 0.95f;
                drainSpeed = 0.45f;
                successMin = 0.46f;
                successMax = 0.66f;
                timeLimit = 7f;
                stageCount = 3;
                break;
        }

        holdRepairPanel.Configure(fillSpeed, drainSpeed, successMin, successMax, timeLimit, stageCount);
        holdRepairPanel.gameObject.SetActive(true);
        holdRepairPanel.Begin(this);
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

        if (sequenceRepairPanel != null)
        {
            sequenceRepairPanel.gameObject.SetActive(false);
        }

        if (holdRepairPanel != null)
        {
            holdRepairPanel.gameObject.SetActive(false);
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