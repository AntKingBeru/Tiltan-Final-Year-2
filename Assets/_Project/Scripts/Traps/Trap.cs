using UnityEngine;

public class Trap : MonoBehaviour
{
    private const int MaxHits = 20;
    
    [Header("Config")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float triggerRadius = 2f;
    [SerializeField] private float avoidChance = 0.2f;
    
    [Header("Detection")]
    [SerializeField] private LayerMask targetLayer;
    
    private float _lastTriggerTime;
    
    protected readonly Collider[] HitsBuffer = new Collider[MaxHits];

    private void Update()
    {
        if (Time.time - _lastTriggerTime < cooldown)
            return;

        TryTrigger();
    }

    private void TryTrigger()
    {
        var count = Physics.OverlapSphereNonAlloc(
            transform.position,
            triggerRadius,
            HitsBuffer,
            targetLayer
        );

        for (var i = 0; i < count; i++)
        {
            var hit = HitsBuffer[i];

            if (hit.TryGetComponent(out Enemy enemy))
            {
                if (ShouldAvoid(enemy))
                    continue;

                Activate(enemy);
                
                _lastTriggerTime = Time.time;
                return;
            }
        }
    }

    protected virtual void Activate(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    private bool ShouldAvoid(Enemy enemy)
    {
        return Random.value < avoidChance;
    }
}