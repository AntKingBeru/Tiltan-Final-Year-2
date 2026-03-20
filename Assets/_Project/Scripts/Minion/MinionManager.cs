using UnityEngine;
using System.Collections.Generic;

public class MinionManager : MonoBehaviour
{
    [SerializeField] private List<Minion> minions;
    
    [SerializeField] private Transform storage;

    public void AssignGatheringTask(GridCell cell)
    {
        foreach (var minion in minions)
        {
            minion.SetGatherTask(cell, storage);
            return;
        }
    }
    
    public void SetAllToPatrol()
    {
        foreach (var minion in minions)
        {
            minion.SetPatrol();
        }
    }
}