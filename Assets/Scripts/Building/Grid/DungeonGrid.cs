using UnityEngine;
using System.Collections.Generic;

public class DungeonGrid : MonoBehaviour
{
    public static DungeonGrid Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 5f;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(100, 100);
    [SerializeField] private Vector3 gridOrigin;
    
    private Dictionary<Vector2Int, GridCell> _cells = new();

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        var minX = -gridSize.x / 2;
        var maxX = minX + gridSize.x;
        
        var minY = -gridSize.y / 2;
        var maxY = minY + gridSize.y;
        
        for (var x = minX; x < maxX; x++)
        {
            for (var y = minY; y < maxY; y++)
            {
                Vector2Int pos = new(x, y);
                _cells[pos] = new GridCell(pos);
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        var offset = worldPos - gridOrigin;
        
        return new Vector2Int(
            Mathf.RoundToInt(offset.x / cellSize),
            Mathf.RoundToInt(offset.z / cellSize)
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return gridOrigin + new Vector3(
            gridPos.x * cellSize,
            0,
            gridPos.y * cellSize
        );
    }

    public bool IsCellOccupied(Vector2Int pos)
    {
        return !_cells.TryGetValue(pos, out var cell) || cell.Occupied;
    }

    public void SetOccupied(Vector2Int pos, bool value)
    {
        if (!_cells.TryGetValue(pos, out var cell)) return;
        
        cell.Occupied = value;
    }
    
    public bool IsAreaFree(Vector2Int start, Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var checkPos = start + new Vector2Int(x, y);
                
                if (IsCellOccupied(checkPos)) return false;
            }
        }

        return true;
    }

    public void SetAreaOccupied(Vector2Int start, Vector2Int size, bool value)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = start + new Vector2Int(x, y);
                
                SetOccupied(pos, value);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        var minX = -gridSize.x / 2;
        var maxX = minX + gridSize.x;
        
        var minY = -gridSize.y / 2;
        var maxY = minY + gridSize.y;
        
        for (var x = minX; x < maxX; x++)
        {
            for (var y = minY; y < maxY; y++)
            {
                var world = new Vector3(x * cellSize, 0, y * cellSize);
                Gizmos.DrawWireCube(world, Vector3.one * cellSize);
            }
        }
    }
#endif
}