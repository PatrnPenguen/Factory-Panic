using UnityEngine;

public class Machine : MonoBehaviour
{
    public bool isBroken = false;

    private Renderer objectRenderer;
    private Color normalColor = Color.white;
    private Color brokenColor = Color.red;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            normalColor = objectRenderer.material.color;
        }

        UpdateVisual();
    }

    public void BreakMachine()
    {
        if (isBroken) return;

        isBroken = true;
        UpdateVisual();
        Debug.Log(gameObject.name + " broke down!");
    }

    public void RepairMachine()
    {
        if (!isBroken) return;

        isBroken = false;
        UpdateVisual();
        Debug.Log(gameObject.name + " repaired!");
    }

    private void UpdateVisual()
    {
        if (objectRenderer == null) return;

        objectRenderer.material.color = isBroken ? brokenColor : normalColor;
    }
}