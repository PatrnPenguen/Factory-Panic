using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRepair : MonoBehaviour
{
    public float interactRange = 1f;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Repair();
        }
    }

    void Repair()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider hit in hits)
        {
            Machine machine = hit.GetComponent<Machine>();
            if (machine != null && machine.isBroken)
            {
                machine.RepairMachine();
                return;
            }
        }
    }
}