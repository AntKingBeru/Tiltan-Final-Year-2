using UnityEngine;

public class GridCell
{
    public Vector2Int Position;
    public CellType CellType;
    public ResourceType ResourceType;

    public GridCellView View;
    
    public bool IsWalkable => CellType == CellType.Cleared;

    public GridCell(Vector2Int position, ResourceType resourceType)
    {
        Position = position;
        ResourceType = resourceType;
        CellType = CellType.Blocked;
    }
}