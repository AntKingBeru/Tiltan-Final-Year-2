using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteAlways]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    
    [Header("Grid Settings")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private float cellSize = 4f;
    
    [Header("Prefabs")]
    [SerializeField] private GridCellView cellPrefab;
    
    [Header("Parents")]
    [SerializeField] private Transform gridParent;
    
    [SerializeField] private InitialRoomPlacer initialRoomPlacer;
    
    private Dictionary<Vector2Int, GridCell> _grid = new();
    
    public IReadOnlyDictionary<Vector2Int, GridCell> Grid => _grid;
    
    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;

    private void Awake()
    {
        Instance = this;
    }
    
    #region Grid Generation

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        ClearGrid();
        
        _grid = new Dictionary<Vector2Int, GridCell>();

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                Vector2Int pos = new(x, y);
                
                var resourceType = Random.value < 0.05f
                    ? ResourceType.Wood
                    : ResourceType.Stone;

                GridCell cell = new(pos, resourceType);

                var worldPos = GridToWorld(pos);

                var view = Instantiate(
                    cellPrefab,
                    worldPos,
                    Quaternion.identity,
                    gridParent
                );
                view.name = $"Cell_{x}_{y}";
                
                view.Initialize(cell);
                
                _grid.Add(pos, cell);
            }
        }
        
        initialRoomPlacer.GenerateInitialRooms();
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        if (!gridParent) return;

        for (var i = gridParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gridParent.GetChild(i).gameObject);
        }
        
        _grid?.Clear();
    }
    
    #endregion
    
    #region Cell Queries

    public GridCell GetCell(Vector2Int pos)
    {
        _grid.TryGetValue(pos, out var cell);
        return cell;
    }

    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.x < width &&
               pos.y >= 0 &&
               pos.y < height;
    }

    public bool IsCellCleared(Vector2Int pos)
    {
        var cell = GetCell(pos);
        return cell is { CellType: CellType.Cleared };
    }

    public bool IsCellWalkable(Vector2Int pos)
    {
        var cell = GetCell(pos);
        return cell is { IsWalkable: true };
    }

    public List<Vector2Int> GetEdgeCells()
    {
        return _grid.Select(kvp => kvp.Key).Where(
                pos => pos.x == 0 || pos.x == width - 1||
                       pos.y == 0 || pos.y == height - 1)
            .ToList();
    }

    public List<Vector2Int> GetValidEnemyEntryCells()
    {
        return (from pos in GetEdgeCells() let cell = GetCell(pos)
            where cell.CellType is CellType.Cleared or CellType.Occupied select pos).ToList();
    }
    
    #endregion
    
    #region Modification

    public void ClearCell(Vector2Int pos)
    {
        var cell = GetCell(pos);
        if (cell is not { CellType: CellType.Blocked })
            return;
        
        cell.CellType = CellType.Cleared;
        cell.View.UpdateView();

        var amount = cell.ResourceType == ResourceType.Wood ? 2 : 1;
        ResourceManager.Instance.Add(cell.ResourceType, amount);
    }

    public void OccupyCell(Vector2Int pos)
    {
        var cell = GetCell(pos);
        if (cell == null)
            return;
        
        cell.CellType = CellType.Occupied;
        cell.View.UpdateView();
    }

    public void ForceClearArea(Vector2Int origin, Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = origin + new Vector2Int(x, y);
                
                if (!IsInsideGrid(pos))
                    continue;
                
                var cell = GetCell(pos);

                if (cell.CellType == CellType.Blocked)
                {
                    cell.CellType = CellType.Cleared;
                    cell.View.UpdateView();
                }
            }
        }
    }
    
    #endregion
    
    #region Room Validation

    public bool CanPlaceRoom(Vector2Int origin, Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = origin + new Vector2Int(x, y);
                
                if (!IsInsideGrid(pos))
                    return false;

                var cell = GetCell(pos);
                if (cell.CellType != CellType.Cleared)
                    return false;
            }
        }

        return true;
    }

    public void OccupyArea(Vector2Int origin, Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                OccupyCell(origin + new Vector2Int(x, y));
            }
        }
    }
    
    #endregion
    
    #region Position Helpers

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * cellSize,
            0,
            gridPos.y * cellSize
        );
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / cellSize),
            Mathf.FloorToInt(worldPos.z / cellSize)
        );
    }
    
    #endregion
}