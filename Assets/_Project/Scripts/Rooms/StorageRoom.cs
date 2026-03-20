public class StorageRoom : Room
{
    protected override void ApplyUpgrade(RoomUpgrade upgrade)
    {
        ResourceManager.Instance.IncreaseCapacity(upgrade.storageBonus);
    }
}