using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour, IDamageable
{
    private const string MinionWalkable = "MinionWalkable";
    private const int MaxHits = 30;
    
    [Header("Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float carryCapacity = 10f;

    [Header("Combat")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float detectionRange = 6f;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    
    private readonly Collider[] _hitsBuffer = new Collider[MaxHits];
    
    private MinionTask _currentTask = MinionTask.Idle;
    
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

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        // TODO: add death logic
    }
    
    #region Setters

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
        _carriedAmount += 1;
        
        _currentTask = MinionTask.Delivering;

        MoveTo(_storage.position);
    }
    
    #endregion
    
    #region Delivering

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
    
    #region Patrol + Combat

    private void HandlePatrol()
    {
        FindEnemy();

        if (_currentEnemy)
        {
            var dist = Vector3.Distance(
                transform.position,
                _currentEnemy.transform.position
            );

            if (dist <= agent.stoppingDistance + 1)
                AttackEnemy();
            else
                MoveTo(_currentEnemy.transform.position);
        }
    }

    private void FindEnemy()
    {
        var count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRange,
            _hitsBuffer
        );

        for (var i = 0; i < count; i++)
        {
            var hit = _hitsBuffer[i];

            if (hit.TryGetComponent(out Enemy enemy))
            {
                _currentEnemy = enemy;
                return;
            }
        }

        _currentEnemy = null;
    }

    private void AttackEnemy()
    {
        if (Time.time - _lastAttackTime < attackRate)
            return;
        
        _lastAttackTime = Time.time;
        
        _currentEnemy?.TakeDamage(attackDamage);
    }

    #endregion
    
    #region Movement

    private void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
    
    #endregion
}