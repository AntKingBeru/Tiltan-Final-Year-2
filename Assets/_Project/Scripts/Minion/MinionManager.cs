using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MinionManager : MonoBehaviour
{
    public static MinionManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private Minion basePrefab;
    [SerializeField] private Minion turtlePrefab;
    [SerializeField] private Minion giantPrefab;
    [SerializeField] private Minion skeletonPrefab;
    [SerializeField] private Minion zombiePrefab;
    
    [SerializeField] private Transform storage;
    [SerializeField] private int maxMinions = 5;
    
    private readonly List<Minion> _minions = new();

    private int _spawnCount;
    
    public Transform Storage => storage ? storage : transform;

    private void Awake()
    {
        Instance = this;
    }

    public List<Minion> GetAll() => _minions;
    
    public void SetStorage(Transform storageRoom) => storage = storageRoom;
    
    #region Spawning

    public void SpawnMinion(Vector3 position)
    {
        if (_minions.Count >= maxMinions)
            return;

        _spawnCount++;

        Minion prefabToSpawn;

        if (_spawnCount % 10 == 0)
        {
            prefabToSpawn = giantPrefab;
        }
        else
            prefabToSpawn = Random.value < 0.5f
                ? turtlePrefab
                : basePrefab;
        
        Spawn(prefabToSpawn, position);
    }
    
    public void SpawnRevivalMinion(Vector3 position, bool isSkeleton)
    {
        if (_minions.Count >= maxMinions)
            return;
        
        var prefab = isSkeleton ? skeletonPrefab : zombiePrefab;
        Spawn(prefab, position);
    }

    private void Spawn(Minion prefab, Vector3 position)
    {
        var minion = Instantiate(
            prefab,
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