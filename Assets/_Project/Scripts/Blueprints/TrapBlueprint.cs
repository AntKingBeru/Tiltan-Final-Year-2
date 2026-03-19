using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Trap Blueprint")]
public class TrapBlueprint : ScriptableObject
{
    public GameObject prefab;
    
    public int stoneCost;
    public int woodCost;
    
    public float damage;
}