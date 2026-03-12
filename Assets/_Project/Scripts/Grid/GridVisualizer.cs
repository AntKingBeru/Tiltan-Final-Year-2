using UnityEngine;
using System.Collections.Generic;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private Mesh quadMesh;
    
    [Header("Materials")]
    [SerializeField] private Material buildMaterial;
    [SerializeField] private Material blockedMaterial;
    [SerializeField] private Material occupiedMaterial;
    [SerializeField] private Material footprintMaterial;
    
    private DungeonGrid _grid;
    private float _cellSize;
    
    private readonly List<Matrix4x4> _buildable = new();
    private readonly List<Matrix4x4> _blocked = new();
    private readonly List<Matrix4x4> _occupied = new();
    private readonly List<Matrix4x4> _footprint = new();

    private const int MAX_BATCH = 1023;

    private void Start()
    {
        _grid = DungeonGrid.Instance;
        _cellSize = _grid.CellSize;
    }

    private void Update()
    {
        CollectCells();
        CollectFootprints();
        DrawBatches();
    }

    private void CollectCells()
    {
        _buildable.Clear();
        _blocked.Clear();
        _occupied.Clear();

        for (var x = 0; x < _grid.Width; x++)
        {
            for (var y = 0; y < _grid.Height; y++)
            {
                var cell = _grid.GetCell(new Vector2Int(x, y));

                var matrix = CreateMatrix(x, y);
                
                if (!cell.IsOccupied) _blocked.Add(matrix);
                else if (cell.IsOccupied) _occupied.Add(matrix);
                else _buildable.Add(matrix);
            }
        }
    }

    private void CollectFootprints()
    {
        _footprint.Clear();

        if (!BuildManager.Instance.CurrentGhost) return;

        var blueprint = BuildManager.Instance.GetSelectedRoom();

        if (!blueprint) return;

        var start = BuildManager.Instance.CurrentGhost.GetGridPosition();

        for (var x = 0; x < blueprint.size.x; x++)
        {
            for (var y = 0; y < blueprint.size.y; y++)
            {
                _footprint.Add(CreateMatrix(start.x + x, start.y + y));
            }
        }
    }

    private Matrix4x4 CreateMatrix(int x, int y)
    {
        var pos = _grid.GridToWorld(new Vector2Int(x, y));
        pos += new Vector3(_cellSize * 0.5f, 0.01f, _cellSize * 0.5f);

        return Matrix4x4.TRS(
            pos,
            Quaternion.Euler(90f, 0f, 0f),
            Vector3.one * _cellSize
        );
    }

    private void DrawBatches()
    {
        Draw(_buildable, buildMaterial);
        Draw(_blocked, blockedMaterial);
        Draw(_occupied, occupiedMaterial);
        Draw(_footprint, footprintMaterial);
    }

    private void Draw(List<Matrix4x4> matrices, Material mat)
    {
        var count = matrices.Count;
        var index = 0;

        while (count > 0)
        {
            var batch = Mathf.Min(count, MAX_BATCH);

            Graphics.DrawMeshInstanced(
                quadMesh,
                0,
                mat,
                matrices.GetRange(index, batch)
            );
            
            count -= batch;
            index += batch;
        }
    }
}