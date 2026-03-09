using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Trap Blueprint")]
public class TrapBlueprint : ScriptableObject
{
    public string trapName;
    public Sprite icon;
    public GameObject prefab;
    public int woodCost;
    public int stoneCost;
}