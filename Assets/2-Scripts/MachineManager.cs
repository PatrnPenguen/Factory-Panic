using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public List<Machine> machines = new List<Machine>();
    public float breakInterval = 4f;

    void Start()
    {
        StartCoroutine(BreakMachinesRoutine());
    }

    IEnumerator BreakMachinesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(breakInterval);
            BreakRandomMachine();
        }
    }

    void BreakRandomMachine()
    {
        List<Machine> healthyMachines = new List<Machine>();

        foreach (Machine machine in machines)
        {
            if (machine != null && !machine.isBroken)
            {
                healthyMachines.Add(machine);
            }
        }

        if (healthyMachines.Count == 0)
        {
            Debug.Log("All machines are already broken!");
            return;
        }

        int randomIndex = Random.Range(0, healthyMachines.Count);
        healthyMachines[randomIndex].BreakMachine();
    }
}