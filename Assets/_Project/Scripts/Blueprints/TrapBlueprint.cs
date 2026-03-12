using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Builder/Trap Blueprint")]
public class TrapBlueprint : ScriptableObject
{
    public string trapName;
    public string description;

    public Sprite icon;

    public GameObject prefab;

    [Header("Cost")]
    public int stoneCost;
    public int woodCost;
    
    [Header("Upgrades")]
    public TrapBlueprint[] upgrades;
}