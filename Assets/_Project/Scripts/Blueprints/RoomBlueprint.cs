using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Builder/Room Blueprint")]
public class RoomBlueprint : ScriptableObject
{
    public string roomName;
    public string description;

    public Sprite icon;

    public GameObject prefab;

    public Vector2Int size;

    [Header("Cost")]
    public int stoneCost;
    public int woodCost;
    
    [Header("Upgrades")]
    public RoomBlueprint[] upgrades;
}