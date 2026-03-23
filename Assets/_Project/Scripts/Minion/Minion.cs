using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Minion : MonoBehaviour, IDamageable
{
    private const string MinionWalkable = "MinionWalkable";
    private const string EnemyWalkable = "EnemyWalkable";
    private const int MaxHits = 20;
    
    [Header("Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private int carryCapacity = 5;

    [Header("Combat")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float detectionRange = 6f;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    
    [Header("References")]
    [SerializeField] private UnitAnimator animator;
    
    private MinionTask _currentTask = MinionTask.Idle;
    
    private readonly Collider[] _hitsBuffer = new Collider[MaxHits];
    private readonly List<MinionTaskData> _taskQueue = new();
    
    private GridCell _targetCell;
    private Transform _storage;

    private int _carriedAmount;
    private ResourceType _carriedType;

    private float _lastAttackTime;
    private Enemy _currentEnemy;

    private void Awake()
    {
        agent.speed = moveSpeed;
        agent.areaMask = 1 << NavMesh.GetAreaFromName(MinionWalkable);
    }

    private void Update()
    {
        FindEnemy();

        if (_currentEnemy)
        {
            _currentTask = MinionTask.Patrol;
            HandleCombat();
            return;
        }
        
        if (_currentTask == MinionTask.Idle && _taskQueue.Count > 0)
        {
            ExecuteNextTask();
        }
        
        switch (_currentTask)
        {
            case MinionTask.Gathering:
                HandleGathering();
                break;
            case MinionTask.Delivering:
                HandleDelivering();
                break;
            case MinionTask.Patrol:
                HandlePatrol();
                break;
        }
    }
    
    #region Task Queue

    public void AddTask(MinionTaskData task)
    {
        _taskQueue.Add(task);
        SortTasks();
    }
    
    private void ExecuteNextTask()
    {
        var task = _taskQueue[0];
        _taskQueue.RemoveAt(0);

        switch (task.TaskType)
        {
            case MinionTask.Gathering:
                SetGatherTask(task.TargetCell, MinionManager.Instance.Storage);
                break;
            case MinionTask.Patrol:
                SetPatrol();
                break;
        }
    }

    public bool IsIdle()
    {
        return _currentTask == MinionTask.Idle && _taskQueue.Count == 0;
    }
    
    private void SortTasks()
    {
        _taskQueue.Sort((a, b) => b.Priority.CompareTo(a.Priority));   
    }
    
    #endregion
    
    #region Tasks
    
    public void SetGatherTask(GridCell cell, Transform storage)
    {
        _targetCell = cell;
        _storage = storage;
        _currentTask = MinionTask.Gathering;
        
        MoveTo(GridManager.Instance.GridToWorld(cell.Position));
    }

    public void SetPatrol()
    {
        _currentTask = MinionTask.Patrol;
        _targetCell = null;
    }
    
    #endregion

    #region Gathering

    private void HandleGathering()
    {
        RemoveArea(EnemyWalkable);
        
        if (_targetCell == null)
            return;

        var dist = Vector3.Distance(
            transform.position,
            GridManager.Instance.GridToWorld(_targetCell.Position)
        );

        if (dist <= agent.stoppingDistance)
            Gather();
    }

    private void Gather()
    {
        if (_targetCell.CellType != CellType.Blocked)
        {
            _currentTask = MinionTask.Idle;
            return;
        }
        
        GridManager.Instance.ClearCell(_targetCell.Position);

        _carriedType = _targetCell.ResourceType;
        _carriedAmount = carryCapacity;
        
        _currentTask = MinionTask.Delivering;

        MoveTo(_storage.position);
    }
    
    private void HandleDelivering()
    {
        if (!_storage)
            return;

        var dist = Vector3.Distance(
            transform.position,
            _storage.position
        );

        if (dist <= agent.stoppingDistance)
            Deliver();
    }

    private void Deliver()
    {
        ResourceManager.Instance.Add(_carriedType, _carriedAmount);

        _carriedAmount = 0;
        
        _currentTask = MinionTask.Idle;
    }
    
    #endregion
    
    #region Combat

    private void HandleCombat()
    {
        if (!_currentEnemy)
            return;

        var dist = Vector3.Distance(
            transform.position,
            _currentEnemy.transform.position
        );

        if (dist <= agent.stoppingDistance + 0.5f)
        {
            animator.SetCombat(true);
            
            if (Time.time - _lastAttackTime >= attackRate)
            {
                animator.TriggerAttack();
                _lastAttackTime = Time.time;
                _currentEnemy.TakeDamage(attackDamage);
            }
        }
        else
            MoveTo(_currentEnemy.transform.position);
    }

    private void HandlePatrol()
    {
        AddArea(EnemyWalkable);
    }

    private void FindEnemy()
    {
        var count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRange,
            _hitsBuffer
        );
        
        var closestDist = float.MaxValue;
        Enemy best = null;
        animator.SetCombat(false);

        for (var i = 0; i < count; i++)
        {
            var hit = _hitsBuffer[i];

            if (hit.TryGetComponent(out Enemy enemy))
            {
                var dist = Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

                if (dist < closestDist)
                {
                    closestDist = dist;
                    best = enemy;
                }
            }
        }

        _currentEnemy = best;
    }
    
    #endregion
    
    #region Movement

    private void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
    
    #endregion
    
    #region Damage + Death
    
    public void TakeDamage(float amount)
    {
        animator.TriggerHit();
        
        health -= amount;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        MinionManager.Instance.UnregisterMinion(this);
        
        animator.Die();
        
        Destroy(gameObject, 5f);   
    }
    
    #endregion
    
    #region Utility

    private void AddArea(string areaName)
    {
        var area = NavMesh.GetAreaFromName(areaName);
        agent.areaMask |= 1 << area;
    }
    
    private void RemoveArea(string areaName)
    {
        var area = NavMesh.GetAreaFromName(areaName);
        agent.areaMask &= ~(1 << area);
    }

    private bool HasArea(string areaName)
    {
        var area = NavMesh.GetAreaFromName(areaName);
        return (agent.areaMask & (1 << area)) != 0;
    }
    
    #endregion
}