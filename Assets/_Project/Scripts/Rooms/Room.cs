using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public Vector2Int Origin { get; private set; }
    public Vector2Int Size { get; private set; }

    public bool BlocksEnemies { get; private set; }
    
    public bool CanUpgrade => _currentLevel < upgrades.Count;
    
    [SerializeField] private List<RoomUpgrade> upgrades;

    private int _currentLevel;

    public void Initialize(Vector2Int origin, Vector2Int size, bool blocksEnemies)
    {
        Origin = origin;
        Size = size;
        BlocksEnemies = blocksEnemies;
    }

    public void Upgrade()
    {
        if (!CanUpgrade)
            return;

        var upgrade = upgrades[_currentLevel];

        if (!ResourceManager.Instance.CanAfford(upgrade.stoneCost, upgrade.woodCost))
            return;

        ResourceManager.Instance.TrySpend(upgrade.stoneCost, upgrade.woodCost);
        
        ApplyUpgrade(upgrade);
        
        _currentLevel++;
    }

    protected virtual void ApplyUpgrade(RoomUpgrade upgrade)
    {
        // Override in children
    }
}