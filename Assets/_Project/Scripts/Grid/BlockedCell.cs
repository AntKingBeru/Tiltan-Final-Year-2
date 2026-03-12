using UnityEngine;

public class BlockedCell : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int resourceAmount = 5;
    
    public ResourceType ResourceType => resourceType;
    public int ResourceAmount => resourceAmount;

    public void Clear()
    {
        ResourceManager.Instance.AddResource(resourceType, resourceAmount);

        Destroy(gameObject);
    }
}