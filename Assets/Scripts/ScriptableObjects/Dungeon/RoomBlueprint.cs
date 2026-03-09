using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Room Blueprint")]
public class RoomBlueprint : ScriptableObject
{
    public string roomName;
	public Sprite icon;
	public Vector2Int size;
	public GameObject prefab;
	public int woodCost;
	public int stoneCost;
	public bool rotatable;
}