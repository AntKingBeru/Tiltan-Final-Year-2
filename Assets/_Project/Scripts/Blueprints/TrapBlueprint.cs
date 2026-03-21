using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Trap Blueprint")]
public class TrapBlueprint : ScriptableObject
{
    public GameObject prefab;
    
    [Header("Cost")]
    public int stoneCost;
    public int woodCost;
    
    [Header("Trap Stats")]
    public float damage;
    public float cooldown;
    public float triggerRadius;

    [Range(0f, 1f)]
    public float avoidChance; // Enemy dodge chance
}