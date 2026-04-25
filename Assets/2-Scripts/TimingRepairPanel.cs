using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TimingRepairPanel : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text stageText;
    public Slider movingSlider;
    public Slider timerSlider;
    public RectTransform successZone;
    public RectTransform sliderFillArea;

    [Header("Settings")]
    public float moveSpeed = 1.5f;
    [Range(0f, 1f)] public float successMin = 0.4f;
    [Range(0f, 1f)] public float successMax = 0.6f;
    public float duration = 4f;
    public int totalStages = 1;

    private MinigameUIManager manager;
    private float currentValue = 0f;
    private int direction = 1;
    private bool isRunning = false;
    private float currentTime = 0f;
    private int currentStage = 1;

    public void Begin(MinigameUIManager uiManager)
    {
        manager = uiManager;
        currentValue = 0f;
        direction = 1;
        currentTime = duration;
        currentStage = 1;
        isRunning = true;

        if (movingSlider != null)
        {
            movingSlider.minValue = 0f;
            movingSlider.maxValue = 1f;
            movingSlider.value = currentValue;
        }

        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = duration;
            timerSlider.value = currentTime;
        }

        RandomizeSuccessZone();
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
            isRunning = false;
            manager.ResolveCurrentMinigame(false);
            return;
        }

        UpdateMovingBar();
        UpdateTimerUI();

        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckResult();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isRunning = false;
            manager.CancelCurrentMinigame();
        }
    }

    private void UpdateMovingBar()
    {
        currentValue += direction * moveSpeed * Time.deltaTime;

        if (currentValue >= 1f)
        {
            currentValue = 1f;
            direction = -1;
        }
        else if (currentValue <= 0f)
        {
            currentValue = 0f;
            direction = 1;
        }

        if (movingSlider != null)
        {
            movingSlider.value = currentValue;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerSlider != null)
        {
            timerSlider.value = currentTime;
        }
    }

    private void CheckResult()
    {
        bool success = currentValue >= successMin && currentValue <= successMax;

        if (!success)
        {
            isRunning = false;
            manager.ResolveCurrentMinigame(false);
            return;
        }

        if (currentStage >= totalStages)
        {
            isRunning = false;
            manager.ResolveCurrentMinigame(true);
            return;
        }

        currentStage++;
        ResetStage();
        RandomizeSuccessZone();
        UpdateSuccessZoneVisual();
        UpdateUI();
    }

    private void ResetStage()
    {
        currentValue = 0f;
        direction = 1;

        if (movingSlider != null)
        {
            movingSlider.value = currentValue;
        }
    }

    private void UpdateUI()
    {
        if (stageText != null)
        {
            stageText.text = "Stage " + currentStage + " / " + totalStages;
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
        float randomCenter = Random.Range(0.2f, 0.8f);

        float halfZone = zoneSize * 0.5f;

        randomCenter = Mathf.Clamp(randomCenter, halfZone, 1f - halfZone);

        successMin = randomCenter - halfZone;
        successMax = randomCenter + halfZone;
    }

    public void Configure(float speed, float minZone, float maxZone, float timeLimit, int stageCount)
    {
        moveSpeed = speed;
        successMin = minZone;
        successMax = maxZone;
        duration = timeLimit;
        totalStages = stageCount;
    }
}