using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GridCellView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Collider cellCollider;

    [Header("Colors")]
    [SerializeField] private Color blockedStoneColor = new(0.4f, 0.4f, 0.4f);
    [SerializeField] private Color blockedWoodColor = new(0.5f, 0.3f, 0.1f);
    [SerializeField] private Color clearedColor = new(0.7f, 0.7f, 0.7f);
    [SerializeField] private Color occupiedColor = new(0f, 0f, 0f, 0f); // invisible
    
    private GridCell _cell;
    private Color _originalColor;

    public void Initialize(GridCell cell)
    {
        _cell = cell;
        _cell.View = this;
        
        meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);

        UpdateView();
    }

    public void UpdateView()
    {
        if (_cell == null)
            return;
        
        switch (_cell.CellType)
        {
            case CellType.Blocked:
                meshRenderer.enabled = true;
                meshRenderer.sharedMaterial.color = 
                    _cell.ResourceType == ResourceType.Stone
                        ? blockedStoneColor
                        : blockedWoodColor;
                cellCollider.enabled = true;
                break;
            case CellType.Cleared:
                meshRenderer.enabled = true;
                meshRenderer.sharedMaterial.color = clearedColor;
                cellCollider.enabled = false;
                break;
            case CellType.Occupied:
            default:
                // Hide mesh completely
                meshRenderer.enabled = false;
                meshRenderer.sharedMaterial.color = occupiedColor;
                cellCollider.enabled = false;
                break;
        }
    }

    public void SetHighlight(Color color)
    {
        _originalColor = meshRenderer.sharedMaterial.color;
        meshRenderer.sharedMaterial.color = color;
    }

    public void ClearHighlight()
    {
        meshRenderer.sharedMaterial.color = _originalColor;
    }
}