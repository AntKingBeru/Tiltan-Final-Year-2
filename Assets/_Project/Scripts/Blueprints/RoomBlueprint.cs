using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Room Blueprint")]
public class RoomBlueprint : ScriptableObject
{
    public GameObject prefab;
    public Vector2Int size;
    
    [Header("Cost")]
    public int stoneCost;
    public int woodCost;
    
    public bool blocksEnemies;
}