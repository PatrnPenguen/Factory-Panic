using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class HoldRepairPanel : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text stageText;
    public Slider pressureSlider;
    public Slider timerSlider;
    public RectTransform successZone;
    public RectTransform sliderFillArea;

    [Header("Settings")]
    public float fillSpeed = 0.65f;
    public float drainSpeed = 0.35f;
    public float duration = 5f;

    [Range(0f, 1f)] public float successMin = 0.45f;
    [Range(0f, 1f)] public float successMax = 0.65f;

    private MinigameUIManager manager;

    private float currentPressure = 0f;
    private float currentTime = 0f;

    private bool isRunning = false;
    private bool wasHolding = false;

    private int currentStage = 1;
    private int totalStages = 1;

    public void Configure(float newFillSpeed, float newDrainSpeed, float minZone, float maxZone, float timeLimit, int stageCount)
    {
        fillSpeed = newFillSpeed;
        drainSpeed = newDrainSpeed;
        successMin = minZone;
        successMax = maxZone;
        duration = timeLimit;
        totalStages = Mathf.Max(1, stageCount);
    }

    public void Begin(MinigameUIManager uiManager)
    {
        manager = uiManager;
        currentStage = 1;

        StartStage();
    }

    private void StartStage()
    {
        currentPressure = 0f;
        currentTime = duration;
        isRunning = true;
        wasHolding = false;

        RandomizeSuccessZone();

        if (pressureSlider != null)
        {
            pressureSlider.minValue = 0f;
            pressureSlider.maxValue = 1f;
            pressureSlider.value = currentPressure;
        }

        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = duration;
            timerSlider.value = currentTime;
        }

        UpdateUI();
        UpdateSuccessZoneVisual();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerUI();
            FailMinigame();
            return;
        }

        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isRunning = false;
            manager.CancelCurrentMinigame();
            return;
        }

        bool isHolding = Keyboard.current.spaceKey.isPressed;

        if (isHolding)
        {
            currentPressure += fillSpeed * Time.deltaTime;
        }
        else
        {
            currentPressure -= drainSpeed * Time.deltaTime;
        }

        currentPressure = Mathf.Clamp01(currentPressure);

        if (pressureSlider != null)
        {
            pressureSlider.value = currentPressure;
        }

        UpdateTimerUI();

        if (wasHolding && !isHolding)
        {
            CheckResult();
            return;
        }

        if (currentPressure >= 1f)
        {
            FailMinigame();
            return;
        }

        wasHolding = isHolding;
    }

    private void CheckResult()
    {
        bool success = currentPressure >= successMin && currentPressure <= successMax;

        if (!success)
        {
            FailMinigame();
            return;
        }

        CompleteStage();
    }

    private void CompleteStage()
    {
        isRunning = false;

        if (currentStage >= totalStages)
        {
            CompleteMinigame();
            return;
        }

        currentStage++;
        StartStage();
    }

    private void CompleteMinigame()
    {
        isRunning = false;
        manager.ResolveCurrentMinigame(true);
    }

    private void FailMinigame()
    {
        isRunning = false;
        manager.ResolveCurrentMinigame(false);
    }

    private void UpdateUI()
    {
        if (stageText != null)
        {
            stageText.text = "Stage " + currentStage + " / " + totalStages;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerSlider != null)
        {
            timerSlider.value = currentTime;
        }
    }

    private void UpdateSuccessZoneVisual()
    {
        if (successZone == null || sliderFillArea == null) return;

        float parentWidth = sliderFillArea.rect.width;
        float zoneWidth = (successMax - successMin) * parentWidth;
        float zoneCenter = ((successMin + successMax) * 0.5f * parentWidth) - (parentWidth * 0.5f);

        Vector2 size = successZone.sizeDelta;
        size.x = zoneWidth;
        successZone.sizeDelta = size;

        Vector2 pos = successZone.anchoredPosition;
        pos.x = zoneCenter;
        successZone.anchoredPosition = pos;
    }

    private void RandomizeSuccessZone()
    {
        float zoneSize = successMax - successMin;
        float halfZone = zoneSize * 0.5f;

        float randomCenter = Random.Range(0.25f, 0.8f);
        randomCenter = Mathf.Clamp(randomCenter, halfZone, 1f - halfZone);

        successMin = randomCenter - halfZone;
        successMax = randomCenter + halfZone;
    }
}