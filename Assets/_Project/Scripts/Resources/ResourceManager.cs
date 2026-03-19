using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Resources")]
    [SerializeField] private int stone;
    [SerializeField] private int wood;

    private void Awake()
    {
        Instance = this;
    }
    
    #region Add Resources

    public void Add(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Stone:
                stone += amount;
                break;
            case ResourceType.Wood:
                wood += amount;
                break;
        }
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
        return true;
    }
    
    #endregion
    
    #region Debug
    
    public int GetStone() => stone;
    public int GetWood() => wood;
    
    #endregion
}