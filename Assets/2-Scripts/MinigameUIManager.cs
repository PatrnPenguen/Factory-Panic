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

        switch (minigameType)
        {
            case MinigameType.TapRepair:
                if (tapRepairPanel != null)
                {
                    tapRepairPanel.gameObject.SetActive(true);
                    tapRepairPanel.Begin(this);
                }
                break;

            case MinigameType.TimingRepair:
                if (timingRepairPanel != null)
                {
                    timingRepairPanel.gameObject.SetActive(true);
                    timingRepairPanel.Begin(this);
                }
                break;

            default:
                ResolveCurrentMinigame(false);
                break;
        }
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