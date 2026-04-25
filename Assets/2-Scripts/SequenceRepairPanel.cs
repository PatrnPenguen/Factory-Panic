using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SequenceRepairPanel : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text sequenceText;
    public Slider timerSlider;

    [Header("Settings")]
    public float duration = 6f;

    private MinigameUIManager manager;
    private Key[] keySequence;
    private int currentIndex = 0;
    private float currentTime = 0f;
    private bool isRunning = false;

    private readonly Key[] possibleKeys =
    {
        Key.W,
        Key.A,
        Key.S,
        Key.D
    };

    public void Configure(int sequenceLength, float timeLimit)
    {
        duration = timeLimit;
        GenerateSequence(sequenceLength);
    }

    public void Begin(MinigameUIManager uiManager)
    {
        manager = uiManager;
        currentIndex = 0;
        currentTime = duration;
        isRunning = true;

        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = duration;
            timerSlider.value = currentTime;
        }

        UpdateUI();
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

        UpdateTimerUI();

        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isRunning = false;
            manager.CancelCurrentMinigame();
            return;
        }

        CheckKeyInput();
    }

    private void CheckKeyInput()
    {
        if (currentIndex >= keySequence.Length) return;

        Key expectedKey = keySequence[currentIndex];

        if (WasKeyPressed(expectedKey))
        {
            currentIndex++;
            UpdateUI();

            if (currentIndex >= keySequence.Length)
            {
                CompleteMinigame();
            }

            return;
        }

        if (WasAnySequenceKeyPressedExcept(expectedKey))
        {
            FailMinigame();
        }
    }

    private bool WasKeyPressed(Key key)
    {
        if (Keyboard.current == null) return false;

        switch (key)
        {
            case Key.W:
                return Keyboard.current.wKey.wasPressedThisFrame;
            case Key.A:
                return Keyboard.current.aKey.wasPressedThisFrame;
            case Key.S:
                return Keyboard.current.sKey.wasPressedThisFrame;
            case Key.D:
                return Keyboard.current.dKey.wasPressedThisFrame;
            default:
                return false;
        }
    }

    private bool WasAnySequenceKeyPressedExcept(Key expectedKey)
    {
        foreach (Key key in possibleKeys)
        {
            if (key == expectedKey) continue;

            if (WasKeyPressed(key))
            {
                return true;
            }
        }

        return false;
    }

    private void GenerateSequence(int sequenceLength)
    {
        sequenceLength = Mathf.Max(1, sequenceLength);
        keySequence = new Key[sequenceLength];

        for (int i = 0; i < keySequence.Length; i++)
        {
            int randomIndex = Random.Range(0, possibleKeys.Length);
            keySequence[i] = possibleKeys[randomIndex];
        }
    }

    private void UpdateUI()
    {
        if (sequenceText != null)
        {
            sequenceText.text = BuildSequenceText();
        }
        
    }

    private string BuildSequenceText()
    {
        if (keySequence == null || keySequence.Length == 0)
        {
            return "";
        }

        string result = "";

        for (int i = 0; i < keySequence.Length; i++)
        {
            string keyName = keySequence[i].ToString();

            if (i < currentIndex)
            {
                result += "<color=#66FF66>" + keyName + "</color>";
            }
            else if (i == currentIndex)
            {
                result += "<color=#FFD966>" + keyName + "</color>";
            }
            else
            {
                result += "<color=#FFFFFF>" + keyName + "</color>";
            }

            if (i < keySequence.Length - 1)
            {
                result += "  ";
            }
        }

        return result;
    }

    private void UpdateTimerUI()
    {
        if (timerSlider != null)
        {
            timerSlider.value = currentTime;
        }
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
}