using UnityEngine;
using System.Collections.Generic;

public class Trap : MonoBehaviour
{
    public int UpgradeLevel { get; private set; }

    private const int MaxHits = 20;
    
    [Header("Config")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float triggerRadius = 2f;
    [SerializeField] private string blueprintId;
    
    [Header("Avoidance")]
    [Range(0f, 1f)]
    [SerializeField] private float avoidChance = 0.2f;
    
    [Header("Targeting")]
    [SerializeField] private LayerMask targetLayer;

    [Header("Upgrades")]
    [SerializeField] private List<TrapUpgrade> upgrades;
    
    private float _lastTriggerTime;
    private int _currentLevel;
    
    protected readonly Collider[] HitsBuffer = new Collider[MaxHits];
    
    public string BlueprintId => blueprintId;

    protected virtual void Update()
    {
        if (Time.time - _lastTriggerTime < cooldown)
            return;

        TryTrigger();
    }

    protected virtual void TryTrigger()
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
        var finalDamage = damage;
        
        if (enemy.EnemyType)
            finalDamage *= (1f - enemy.EnemyType.trapResistance);
        
        enemy.TakeDamage(finalDamage);
    }

    protected virtual bool ShouldAvoid(Enemy enemy)
    {
        if (enemy.EnemyType && enemy.EnemyType.ignoreTraps)
            return true;
        
        return Random.value < avoidChance;
    }

    #region Upgrades
    
    public void Upgrade()
    {
        if (_currentLevel >= upgrades.Count)
            return;

        var upgrade = upgrades[_currentLevel];

        damage += upgrade.damageBonus;
        cooldown = Mathf.Max(0.2f, cooldown - upgrade.cooldownReduction);
        
        _currentLevel++;
    }

    public void SetUpgradeLevel(int level)
    {
        UpgradeLevel = level;
    }
    
    #endregion
}