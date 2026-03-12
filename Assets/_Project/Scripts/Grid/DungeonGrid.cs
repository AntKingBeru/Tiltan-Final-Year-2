using UnityEngine;

public class DungeonGrid : MonoBehaviour
{
    public static DungeonGrid Instance;

    [Header("Grid Settings")]
    [SerializeField] private int width = 50; 
    [SerializeField] private int height = 50;
    [SerializeField] private float cellSize = 4f;
    
    [SerializeField] private GameObject clearedCellPrefab;

    private GridCell[,] _grid;
    
    public int Width => width;
    public int Height => height;

    private void Awake()
    {
        Instance = this;

        BuildGridFromScene();

        RegisterExistingRooms();
    }

    private void BuildGridFromScene()
    {
        _grid = new GridCell[width, height];
        
        var cells = FindObjectsByType<BlockedCell>(FindObjectsSortMode.None);

        foreach (var cell in cells)
        {
            var pos = WorldToGrid(cell.transform.position);
            
            if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height) continue;
            
            _grid[pos.x, pos.y] = new GridCell(pos, cell.ResourceType);
        }
    }

    private void RegisterExistingRooms()
    {
        var rooms = FindObjectsByType<Room>(FindObjectsSortMode.None);

        foreach (var room in rooms)
        {
            var gridPos = WorldToGrid(room.transform.position);

            RegisterRoom(
                gridPos,
                room.Size,
                room.gameObject
            );
        }
    }

    public GridCell GetCell(Vector2Int position)
    {
        if (position.x < 0 || position.x >= width ||
            position.y < 0 || position.y >= height)
        {
            return null;
        }
        
        return _grid[position.x, position.y];
    }

    public bool ClearCell(Vector2Int position)
    {
        var cell = GetCell(position);

        if (cell == null) return false;
        
        if (cell.IsCleared) return false;
        
        cell.IsCleared = true;
        
        ResourceManager.Instance.AddResource(cell.ResourceType, 1);
        
        var worldPos = GridToWorld(position);
        Instantiate(clearedCellPrefab, worldPos, Quaternion.identity);

        return true;
    }

    public bool AreCellsCleared(Vector2Int start, Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = start + new Vector2Int(x, y);
                
                var cell = GetCell(pos);

                if (cell is not { IsCleared: true } || cell.IsOccupied) return false;
            }
        }
        
        return true;
    }

    public void RegisterRoom(
        Vector2Int start,
        Vector2Int size,
        GameObject room)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = start + new Vector2Int(x, y);
                
                var cell = GetCell(pos);
                
                if (cell == null) continue;
                
                cell.IsCleared = true;
                cell.IsOccupied = true;
                cell.Room = room;
            }
        }
    }
    
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        var x = Mathf.FloorToInt(worldPosition.x / cellSize);
        var y = Mathf.FloorToInt(worldPosition.z / cellSize);
        
        return new Vector2Int(x, y);
    }
    
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        var worldX = gridPosition.x * cellSize;
        var worldZ = gridPosition.y * cellSize;
        
        return new Vector3(worldX, 0, worldZ);
    }
    
    public float CellSize => cellSize;
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (var x = 0; x <= width; x++)
        {
            var start = new Vector3(x * cellSize, 0, 0);
            var end = new Vector3(x * cellSize, 0, height * cellSize);
            
            Gizmos.DrawLine(start, end);
        }

        for (var y = 0; y <= height; y++)
        {
            var start = new Vector3(0, 0, y * cellSize);
            var end = new Vector3(width * cellSize, 0, y * cellSize);
            
            Gizmos.DrawLine(start, end);
        }
    }
    #endif
}