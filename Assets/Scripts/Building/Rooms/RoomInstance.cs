using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    public RoomBlueprint blueprint;

    public Vector2Int GridPosition { get; private set; }
    
    [SerializeField] private RoomAnchor[] anchors;

    public void Initialize(RoomBlueprint schematic, Vector2Int gridPosition)
    {
        blueprint = schematic;
        GridPosition = gridPosition;
    }

    public RoomAnchor[] GetAnchors()
    {
        return anchors;
    }
}