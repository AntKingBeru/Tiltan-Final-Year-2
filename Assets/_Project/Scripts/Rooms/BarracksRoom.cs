public class BarracksRoom : Room
{
    protected override void ApplyUpgrade(RoomUpgrade upgrade)
    {
        MinionManager.Instance.IncreaseMaxMinions(upgrade.minionCapacityBonus);
    }
}