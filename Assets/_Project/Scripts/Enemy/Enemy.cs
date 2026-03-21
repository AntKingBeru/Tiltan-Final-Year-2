using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private const string EnemyWalkable = "EnemyWalkable";
    
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRate = 1f;
    
    [Header("Type")]
    [SerializeField] private EnemyType enemyType;
    
    [Header("Death")]
    [SerializeField] private GameObject corpsePrefab;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float fleeThreshold = 20f;
    [SerializeField] private float fleeDistance = 6f;
    
    public EnemyType EnemyType => enemyType;

    private float _health;
    private Core _core;

    private IDamageable _target;
    private Transform _targetTransform;
    
    private float _lastAttackTime;

    private bool _isFleeing;
    
    private Vector3 _formationOffset;
    
    private readonly ThreatTracker _threatTracker = new();

    private void Awake()
    {
        _health = maxHealth;
        
        agent.areaMask = 1 << NavMesh.GetAreaFromName(EnemyWalkable);
        
        _formationOffset = Random.insideUnitSphere * 2f;
        _formationOffset.y = 0;
    }
    
    public void Initialize(Core core)
    {
        _core = core;
        SetTarget(core, core.transform);
    }

    private void Update()
    {
        if (_health <= fleeThreshold && !_isFleeing)
        {
            HandleFlee();
            return;
        }
        
        UpdateThreatTarget();
        
        if (!_targetTransform)
            return;

        var dist = Vector3.Distance(
            transform.position,
            _core.transform.position
        );

        if (dist <= agent.stoppingDistance + 0.5f)
            Attack();
        else
            MoveTo(_core.transform.position + _formationOffset);
    }
    
    public NavMeshAgent GetAgent() => agent;
    
    #region Threat System
    
    public void RegisterThreat(IDamageable source, float amount)
    {
        _threatTracker.AddThreat(source, amount);
    }

    private void UpdateThreatTarget()
    {
        _threatTracker.DecayThreat(5f);

        var target = _threatTracker.GetHighestThreat();
        
        if (target != null)
            SetTarget(target, ((MonoBehaviour)target).transform);
        else
            SetTarget(_core, _core.transform);
    }
    
    private void SetTarget(IDamageable target, Transform t)
    {
        _target = target;
        _targetTransform = t;
    }
    
    #endregion
    
    #region Combat
    
    private void Attack()
    {
        if (Time.time - _lastAttackTime < attackRate)
            return;
        
        _lastAttackTime = Time.time;
        
        _target?.TakeDamage(damage);
    }
    
    #endregion
    
    #region Movement

    private void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
    
    #endregion
    
    #region Flee

    private void HandleFlee()
    {
        if (_isFleeing)
            return;
        
        _isFleeing = true;
        
        var dir = (transform.position - _targetTransform.position).normalized;
        var fleePos = _targetTransform.position + dir * fleeDistance;
        
        MoveTo(fleePos);
    }

    private bool IsFearless()
    {
        return enemyType && enemyType.fearless;
    }
    
    #endregion
    
    #region Damage + Death
    
    public void TakeDamage(float amount)
    {
        if (enemyType)
            amount *= (1f - enemyType.trapResistance);
        
        _health -= amount;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        if (corpsePrefab)
            Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
    
    #endregion   
}