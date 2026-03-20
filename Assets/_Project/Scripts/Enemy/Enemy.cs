using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private const string EnemyWalkable = "EnemyWalkable";
    private const int MaxHits = 20;
    
    [Header("Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float detectionRange = 5f;
    
    [Header("Death")]
    [SerializeField] private GameObject corpsePrefab;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    
    private Core _core;
    
    private float _lastAttackTime;

    private IDamageable _targetDamageable;
    private Transform _targetTransform;
    
    private readonly Collider[] _hitsBuffer = new Collider[MaxHits];

    private void Awake()
    {
        agent.areaMask = 1 << NavMesh.GetAreaFromName(EnemyWalkable);
    }
    
    public void Initialize(Core core)
    {
        _core = core;
        SetTarget(core, core.transform);
    }

    private void Update()
    {
        FindTarget();
        
        if (!_targetTransform)
            return;

        var dist = Vector3.Distance(
            transform.position,
            _core.transform.position
        );

        if (dist <= agent.stoppingDistance)
            Attack();
        else
            agent.SetDestination(_core.transform.position);
    }
    
    #region Targeting

    private void FindTarget()
    {
        var count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRange,
            _hitsBuffer
        );
        
        var closestDist = float.MaxValue;
        Minion best = null;

        for (var i = 0; i < count; i++)
        {
            var hit = _hitsBuffer[i];
            
            if (hit.TryGetComponent(out Minion minion))
            {
                var dist = Vector3.Distance(
                    transform.position,
                    minion.transform.position
                );
                
                if (dist < closestDist)
                {
                    closestDist = dist;
                    best = minion;
                }
            }
            
            if (best)
                SetTarget(best, best.transform);
            else
                SetTarget(_core, _core.transform);
        }
    }
    
    private void SetTarget(IDamageable damageable, Transform target)
    {
        _targetDamageable = damageable;
        _targetTransform = target;
    }
    
    #endregion
    
    #region Combat

    private void Attack()
    {
        if (Time.time - _lastAttackTime < attackRate)
            return;
        
        _lastAttackTime = Time.time;
        
        _core.TakeDamage(damage);
    }
    
    #endregion
    
    #region Damage + Death
    
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
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