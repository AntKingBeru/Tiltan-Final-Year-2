using UnityEngine;
using System.Collections.Generic;

public class MinionManager : MonoBehaviour
{
    public static MinionManager Instance { get; private set; }

    [SerializeField] private Minion minionPrefab;
    [SerializeField] private Transform storage;
    [SerializeField] private int maxMinions = 5;
    
    private readonly List<Minion> _minions = new();

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnMinion(Vector3 position)
    {
        if (_minions.Count >= maxMinions)
            return;

        var minion = Instantiate(
            minionPrefab,
            position,
            Quaternion.identity
        );
        _minions.Add(minion);
    }

    public void UnregisterMinion(Minion minion)
    {
        _minions.Remove(minion);
    }
    
    public void AssignGatheringTask(GridCell cell)
    {
        foreach (var minion in _minions)
        {
            minion.SetGatherTask(cell, storage);
            return;
        }
    }
    
    public void SetAllToPatrol()
    {
        foreach (var minion in _minions)
        {
            minion.SetPatrol();
        }
    }
}