using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRepair : MonoBehaviour
{
    public float interactRange = 2.2f;

    void Update()
    {
        if (FactoryManager.Instance != null && FactoryManager.Instance.IsGameOver())
        {
            return;
        }
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteractWithMachine();
        }
    }

    private void TryInteractWithMachine()
    {
        if (MinigameUIManager.Instance != null && MinigameUIManager.Instance.IsMinigameOpen())
        {
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);

        Machine closestMachine = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Machine machine = hit.GetComponent<Machine>();
            if (machine == null) continue;
            if (!machine.CanInteract()) continue;

            float distance = Vector3.Distance(transform.position, machine.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMachine = machine;
            }
        }

        if (closestMachine != null)
        {
            closestMachine.StartMinigame();
        }
    }
}