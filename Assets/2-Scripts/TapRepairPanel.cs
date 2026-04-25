using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TapRepairPanel : MonoBehaviour
{
    [Header("UI")]
    public Slider timerSlider;
    public Image repairProgressImage;
    public Sprite[] progressSprites;
    public Image vana;

    [Header("Settings")]
    public int requiredPressCount;
    public float duration;

    private MinigameUIManager manager;
    private int currentPressCount;
    private float currentTime;
    private bool isRunning = false;

    private MinigameDifficulty currentDifficulty = MinigameDifficulty.Medium;
    private int pressesPerStage = 1;

    public void Configure(int pressCount, float timeLimit, MinigameDifficulty difficulty)
    {
        requiredPressCount = pressCount;
        duration = timeLimit;
        currentDifficulty = difficulty;

        switch (currentDifficulty)
        {
            case MinigameDifficulty.Easy:
                pressesPerStage = 1;
                break;

            case MinigameDifficulty.Medium:
                pressesPerStage = 2;
                break;

            case MinigameDifficulty.Hard:
                pressesPerStage = 3;
                break;

            default:
                pressesPerStage = 1;
                break;
        }
    }

    public void Begin(MinigameUIManager uiManager)
    {
        manager = uiManager;
        currentPressCount = 0;
        currentTime = duration;
        isRunning = true;

        UpdateUI();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (vana != null)
                {
                    int rotation = 20;
                    vana.transform.Rotate(0, 0, rotation);
                    rotation += 20;
                }
                currentPressCount++;
                UpdateUI();

                if (currentPressCount >= requiredPressCount)
                {
                    isRunning = false;
                    manager.ResolveCurrentMinigame(true);
                    return;
                }
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                isRunning = false;
                manager.CancelCurrentMinigame();
                return;
            }
        }

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            UpdateUI();
            manager.ResolveCurrentMinigame(false);
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerSlider != null)
        {
            timerSlider.maxValue = duration;
            timerSlider.value = currentTime;
        }

        UpdateRepairImage();
    }

    private void UpdateRepairImage()
    {
        if (repairProgressImage == null) return;
        if (progressSprites == null || progressSprites.Length == 0) return;

        int stageIndex = currentPressCount / pressesPerStage;

        if (currentPressCount >= requiredPressCount)
        {
            stageIndex = progressSprites.Length - 1;
        }

        stageIndex = Mathf.Clamp(stageIndex, 0, progressSprites.Length - 1);
        repairProgressImage.sprite = progressSprites[stageIndex];
    }
}