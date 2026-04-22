using UnityEngine;
using UnityEngine.UI;

public class MachineTimerUI : MonoBehaviour
{
    public Machine targetMachine;
    public Canvas worldCanvas;
    public Slider timerSlider;
    

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (FactoryManager.Instance != null && FactoryManager.Instance.IsGameOver())
        {
            worldCanvas.gameObject.SetActive(false);
            return;
        }
        if (targetMachine == null) return;

        UpdateVisibility();
        UpdateSlider();
    }
    
    private void UpdateVisibility()
    {
        bool shouldShow = targetMachine.currentState == MachineState.Broken;

        if (worldCanvas != null && worldCanvas.gameObject.activeSelf != shouldShow)
        {
            worldCanvas.gameObject.SetActive(shouldShow);
        }
    }

    private void UpdateSlider()
    {
        if (timerSlider == null) return;
        if (targetMachine.currentState != MachineState.Broken) return;

        timerSlider.value = targetMachine.GetBrokenTimeNormalized();
    }
}