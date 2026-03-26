using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public string BlueprintId { get; private set; }
    public Vector2Int Origin { get; private set; }
    public Vector2Int Size { get; private set; }
    public int RotationIndex { get; private set; }

    public bool BlocksEnemies { get; private set; }
    
    public bool CanUpgrade => UpgradeLevel < upgrades.Count;
    
    [SerializeField] private List<RoomUpgrade> upgrades;

    public int UpgradeLevel { get; private set; }

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

        var upgrade = upgrades[UpgradeLevel];

        if (!ResourceManager.Instance.CanAfford(upgrade.stoneCost, upgrade.woodCost))
            return;

        ResourceManager.Instance.TrySpend(upgrade.stoneCost, upgrade.woodCost);
        
        ApplyUpgrade(upgrade);
        
        UpgradeLevel++;
    }

    public void SetUpgradeLevel(int level)
    {
        UpgradeLevel = 0;

        for (var i = 0; i < level; i++)
        {
            if (i >= upgrades.Count)
                break;
            
            ApplyUpgrade(upgrades[i]);
            UpgradeLevel++;
        }
    }

    private void OnMouseDown()
    {
        if (!BuildManager.Instance.IsUpgradeMode)
            BuildManager.Instance.UpgradeUI.Show(this);
    }

    protected virtual void ApplyUpgrade(RoomUpgrade upgrade)
    {
        // Override in children
    }
}