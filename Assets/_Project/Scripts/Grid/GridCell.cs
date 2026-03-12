using UnityEngine;

public class GridCell
{
    public Vector2Int GridPosition;

    public bool IsCleared;
    public bool IsOccupied;
    
    public ResourceType ResourceType;

    public GameObject Room;
    public GameObject Trap;

    public GridCell(Vector2Int position, ResourceType resourceType)
    {
        GridPosition = position;
        ResourceType = resourceType;
        
        IsCleared = false;
        IsOccupied = false;
    }
}