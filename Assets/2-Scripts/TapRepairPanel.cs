using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TapRepairPanel : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text titleText;
    public TMP_Text infoText;
    public Slider progressSlider;
    public Slider timerSlider;

    [Header("Settings")]
    public int requiredPressCount = 8;
    public float duration = 4f;

    private MinigameUIManager manager;
    private int currentPressCount;
    private float currentTime;
    private bool isRunning = false;

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
        if (titleText != null)
        {
            titleText.text = "Tap Repair";
        }

        if (infoText != null)
        {
            infoText.text = "Press SPACE rapidly: " + currentPressCount + " / " + requiredPressCount;
        }

        if (progressSlider != null)
        {
            progressSlider.maxValue = requiredPressCount;
            progressSlider.value = currentPressCount;
        }

        if (timerSlider != null)
        {
            timerSlider.maxValue = duration;
            timerSlider.value = currentTime;
        }
    }
}