using UnityEngine;

public class RoomGhost : MonoBehaviour
{
    private RoomBlueprint _blueprint;
    private Camera _cam;
    private bool _canPlace;
    private Renderer[] _renderers;
    private Material _validMaterial;
    private Material _invalidMaterial;
    private float _cellSize;

    public void Initialize(
        RoomBlueprint blueprint,
        Camera cam,
        GhostMaterialSettings materials)
    {
        _blueprint = blueprint;
        _cam = cam;
        
        _validMaterial = materials.validMaterial;
        _invalidMaterial = materials.invalidMaterial;
        
        _renderers = GetComponentsInChildren<Renderer>();
        
        _cellSize = DungeonGrid.Instance.CellSize;
    }

    private void Update()
    {
        UpdatePosition();
        ValidatePlacement();
        UpdateMaterial();
    }

    private void UpdatePosition()
    {
        var gridPos = GridUtility.GetMouseGridPosition(_cam);
        
        var basePos = DungeonGrid.Instance.GridToWorld(gridPos);

        var offset = new Vector3(
            (_blueprint.size.x - 1) * _cellSize * 0.5f,
            0,
            (_blueprint.size.y - 1) * _cellSize * 0.5f
        );
        
        transform.position = basePos + offset;
    }

    private void ValidatePlacement()
    {
        var gridPos = GridUtility.GetMouseGridPosition(_cam);

        var gridValid = DungeonGrid.Instance.AreCellsCleared(
            gridPos,
            _blueprint.size
        );
        
        var affordable = ResourceManager.Instance.CanAfford(
            _blueprint.stoneCost,
            _blueprint.woodCost
        );
        
        _canPlace = gridValid && affordable;
    }

    private void UpdateMaterial()
    {
        var material = _canPlace ? _validMaterial : _invalidMaterial;

        foreach (var r in _renderers) r.material = material;
    }
    
    public bool CanPlace() => _canPlace;
    
    public Vector2Int GetGridPosition() => GridUtility.GetMouseGridPosition(_cam);
}