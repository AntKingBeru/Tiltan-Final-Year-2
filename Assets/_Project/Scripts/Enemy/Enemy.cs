using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private const string EnemyWalkable = "EnemyWalkable";
    
    [Header("Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRate = 1f;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    
    private Core _core;
    private float _lastAttackTime;
    
    public void Initialize(Core core)
    {
        _core = core;
        agent.SetDestination(core.transform.position);
        agent.areaMask = 1 << NavMesh.GetAreaFromName(EnemyWalkable);
    }

    private void Update()
    {
        if (!_core)
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

    private void Attack()
    {
        if (Time.time - _lastAttackTime < attackRate)
            return;
        
        _lastAttackTime = Time.time;
        
        _core.TakeDamage(damage);
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        // TODO: Spawn corpse
        Destroy(gameObject);
    }
}