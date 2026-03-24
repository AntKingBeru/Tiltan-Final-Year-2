using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public string BlueprintId { get; private set; }
    public Vector2Int Origin { get; private set; }
    public Vector2Int Size { get; private set; }
    public int RotationIndex { get; private set; }

    public bool BlocksEnemies { get; private set; }
    
    public bool CanUpgrade => _currentLevel < upgrades.Count;
    
    [SerializeField] private List<RoomUpgrade> upgrades;

    private int _currentLevel;
    
    public int UpgradeLevel => _currentLevel;

    public void Initialize(Vector2Int origin, Vector2Int size, bool blocksEnemies, string blueprintId, int rotationIndex)
    {
        Origin = origin;
        Size = size;
        BlocksEnemies = blocksEnemies;
        
        BlueprintId = blueprintId;
        RotationIndex = rotationIndex;
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

    public void SetUpgradeLevel(int level)
    {
        _currentLevel = 0;

        for (var i = 0; i < level; i++)
        {
            if (i >= upgrades.Count)
                break;
            
            ApplyUpgrade(upgrades[i]);
            _currentLevel++;
        }
    }

    private void OnMouseDown()
    {
        if (!BuildManager.Instance.IsBuildMode)
            BuildManager.Instance.UpgradeUI.Show(this);
    }

    protected virtual void ApplyUpgrade(RoomUpgrade upgrade)
    {
        // Override in children
    }
}