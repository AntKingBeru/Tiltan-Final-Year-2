using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MinionManager : MonoBehaviour
{
    public static MinionManager Instance { get; private set; }

    [SerializeField] private Minion minionPrefab;
    [SerializeField] private Transform storage;
    [SerializeField] private int maxMinions = 5;
    
    private readonly List<Minion> _minions = new();
    
    public Transform Storage => storage;

    private void Awake()
    {
        Instance = this;
    }
    
    #region Spawning

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
    
    public void IncreaseMaxMinions(int bonus)
    {
        maxMinions += bonus;
    }
    
    #endregion
    
    #region Task Assignment
    
    public void AssignGatheringTask(GridCell cell)
    {
        var minion = GetFreeMinion();

        if (!minion)
            return;
        
        minion.AddTask(new MinionTaskData(
            MinionTask.Gathering,
            cell,
            50
        ));
    }
    
    public void SetAllToPatrol()
    {
        foreach (var minion in _minions)
        {
            minion.AddTask(new MinionTaskData(
                MinionTask.Patrol,
                null,
                100
            ));
        }
    }
    
    #endregion
    
    #region Auto Assignment

    public void AutoAssignGathering()
    {
        foreach (var cell in GridManager.Instance.Grid.Values)
        {
            if (cell.CellType == CellType.Blocked)
            {
                AssignGatheringTask(cell);
                return;
            }
        }
    }

    private Minion GetFreeMinion()
    {
        return _minions.FirstOrDefault(minion => minion.IsIdle());
    }

    #endregion
}