using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Stone { get; private set; }
    public int Wood { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Stone:
                Stone += amount;
                break;
            case ResourceType.Wood:
                Wood += amount;
                break;
        }
        
        Debug.Log($"Resources -> Stone: {Stone} | Wood: {Wood}");
    }

    public bool CanAfford(int stoneCost, int woodCost)
    {
        return Stone >= stoneCost && Wood >= woodCost;
    }

    public bool Spend(int stoneCost, int woodCost)
    {
        if (!CanAfford(stoneCost, woodCost)) return false;
        
        Stone -= stoneCost;
        Wood -= woodCost;
        
        return true;
    }
}