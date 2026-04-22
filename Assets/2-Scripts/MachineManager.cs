using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [Header("Machine List")]
    public List<Machine> machines = new List<Machine>();

    [Header("Break Timing")]
    public float levelStartDelay = 10f;
    public float waveInterval = 6f;

    [Header("Break Amount")]
    public int minMachinesPerWave = 1;
    public int maxMachinesPerWave = 2;

    [Header("Break Mode")]
    public bool useSequentialOrder = false;

    private int sequentialIndex = 0;

    void Start()
    {
        StartCoroutine(BreakRoutine());
    }

    private IEnumerator BreakRoutine()
    {
        yield return new WaitForSeconds(levelStartDelay);

        while (true)
        {
            if (FactoryManager.Instance != null && FactoryManager.Instance.IsGameOver())
            {
                yield break;
            }

            TriggerBreakWave();

            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void TriggerBreakWave()
    {
        List<Machine> availableMachines = GetAvailableMachines();

        if (availableMachines.Count == 0)
        {
            Debug.Log("No available machines to break right now.");
            return;
        }

        int breakCount = Random.Range(minMachinesPerWave, maxMachinesPerWave + 1);
        breakCount = Mathf.Min(breakCount, availableMachines.Count);

        if (useSequentialOrder)
        {
            BreakSequential(availableMachines, breakCount);
        }
        else
        {
            BreakRandom(availableMachines, breakCount);
        }
    }

    private List<Machine> GetAvailableMachines()
    {
        List<Machine> available = new List<Machine>();

        foreach (Machine machine in machines)
        {
            if (machine != null && machine.CanBreak())
            {
                available.Add(machine);
            }
        }

        return available;
    }

    private void BreakRandom(List<Machine> availableMachines, int breakCount)
    {
        List<Machine> tempList = new List<Machine>(availableMachines);

        for (int i = 0; i < breakCount; i++)
        {
            if (tempList.Count == 0) break;

            int randomIndex = Random.Range(0, tempList.Count);
            Machine selectedMachine = tempList[randomIndex];

            selectedMachine.BreakMachine();
            tempList.RemoveAt(randomIndex);
        }
    }

    private void BreakSequential(List<Machine> availableMachines, int breakCount)
    {
        int broken = 0;
        int safety = 0;

        while (broken < breakCount && safety < machines.Count * 2)
        {
            if (machines.Count == 0) break;

            Machine machine = machines[sequentialIndex];

            sequentialIndex++;
            if (sequentialIndex >= machines.Count)
            {
                sequentialIndex = 0;
            }

            if (machine != null && machine.CanBreak())
            {
                machine.BreakMachine();
                broken++;
            }

            safety++;
        }
    }
}