using UnityEngine;

public class GridCell
{
    public Vector2Int Position;
    public bool Occupied;

    public GridCell(Vector2Int pos)
    {
        Position = pos;
        Occupied = false;
    }
}