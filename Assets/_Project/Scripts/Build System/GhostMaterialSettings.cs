using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Builder/Ghost Materials")]
public class GhostMaterialSettings : ScriptableObject
{
    public Material validMaterial;
    public Material invalidMaterial;
}