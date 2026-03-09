using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Room Blueprint")]
public class RoomBlueprint : ScriptableObject
{
    [Header("Basic Info")]
    public string roomName;
    public string roomDescription;
    public Sprite icon;
    
    [Header("Grid")]
	public Vector2Int size;
    
    [Header("Prefab")]
	public GameObject prefab;
    
    [Header("Costs")]
	public int woodCost;
	public int stoneCost;
    
    [Header("Placement")]
	public bool rotatable = true;
}