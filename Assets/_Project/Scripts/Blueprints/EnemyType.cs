using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Enemy Type")]
public class EnemyType : ScriptableObject
{
    public float healthMultiplier = 1f;

    [Header("Resistance")]
    public float trapResistance;
    public float slowResistance;

    [Header("Behavior")]
    public bool ignoreTraps;
    public bool fearless;
}