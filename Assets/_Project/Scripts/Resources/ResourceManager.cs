using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    public event Action OnResourcesChanged;

    [Header("Resources")]
    [SerializeField] private int stone;
    [SerializeField] private int wood;

    private int _maxCapacity = 50;
    
    private void Awake()
    {
        Instance = this;
    }

    private void NotifyChanged()
    {
        OnResourcesChanged?.Invoke();
    }
    
    #region Add Resources

    public void Add(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Stone:
                stone += amount;
                if (stone > _maxCapacity)
                    stone = _maxCapacity;
                break;
            case ResourceType.Wood:
                wood += amount;
                if (wood > _maxCapacity)
                    wood = _maxCapacity;
                break;
        }
        
        NotifyChanged();
    }

    public void IncreaseCapacity(int bonus)
    {
        _maxCapacity += bonus;
        NotifyChanged();
    }

    public void Set(int newStone, int newWood)
    {
        stone = newStone;
        wood = newWood;
        
        NotifyChanged();
    }
    
    #endregion
    
    #region Spend Resources

    public bool CanAfford(int stoneCost, int woodCost)
    {
        return stone >= stoneCost && wood >= woodCost;
    }

    public bool TrySpend(int stoneCost, int woodCost)
    {
        if (!CanAfford(stoneCost, woodCost))
            return false;
        
        stone -= stoneCost;
        wood -= woodCost;
        
        NotifyChanged();
        return true;
    }
    
    #endregion
    
    #region Debug
    
    public int GetStone() => stone;
    public int GetWood() => wood;
    
    #endregion
}