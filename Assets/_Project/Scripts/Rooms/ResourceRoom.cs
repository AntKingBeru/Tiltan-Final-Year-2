using UnityEngine;

public class ResourceRoom : Room
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amountPerCycle = 5;
    [SerializeField] private float interval = 5f;

    private float _timer;

    public bool HasResources { get; private set; }
    public int StoredAmount { get; private set; }
    
    protected override void ApplyUpgrade(RoomUpgrade upgrade)
    {
        amountPerCycle += upgrade.resourceGenerationBonus;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= interval)
        {
            _timer = 0;
            Generate();
        }
    }

    private void Generate()
    {
        StoredAmount += amountPerCycle;
        HasResources = true;
    }

    public int Collect()
    {
        var amount = StoredAmount;
        StoredAmount = 0;
        HasResources = false;
        return amount;
    }
}