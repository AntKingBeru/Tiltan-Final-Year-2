using UnityEngine;
using UnityEngine.InputSystem;

public class GridCursor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera dungeonCamera;
    [SerializeField] private LayerMask gridLayer;
    
    [Header("Visual")]
    [SerializeField] private GameObject visual;
    
    [Header("Colors")]
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;
    
    private GridCell _currentCell;
    private GridCellView _currentView;

    private void Update()
    {
        if (!BuildManager.Instance.IsBuildMode)
        {
            SetVisible(false);
            return;
        }
        
        SetVisible(true);

        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        var ray = dungeonCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit, 100f, gridLayer))
        {
            ClearHighlight();
            return;
        }

        var gridPos = GridManager.Instance.WorldToGrid(hit.point);
        var cell = GridManager.Instance.GetCell(gridPos);

        if (cell == null)
        {
            ClearHighlight();
            return;
        }
        
        // Move cursor
        var pos = GridManager.Instance.GridToWorld(gridPos);
        pos += new Vector3(2f, 0f, 2f);
        transform.position = pos;
        
        // Highlight logic
        if (_currentCell != cell)
        {
            ClearHighlight();

            _currentCell = cell;
            _currentView = cell.View;

            HighlightCell(cell);
        }
    }

    private void HighlightCell(GridCell cell)

    {
        var isValid = cell.CellType == CellType.Cleared;

        _currentView.SetHighlight(isValid ? validColor : invalidColor);
    }

    private void ClearHighlight()
    {
        if (_currentView)
        {
            _currentView.ClearHighlight();
            _currentView = null;
            _currentCell = null;
        }
    }

    private void SetVisible(bool value)
    {
        if (visual)
            visual.SetActive(value);
    }
}