using UnityEngine;
using UnityEngine.InputSystem;

public class TrapGhost : MonoBehaviour
{
    private TrapBlueprint _blueprint;
    private Camera _cam;
    private TrapAnchor _anchor;
    private Renderer[] _renderers;
    private Material _validMaterial;
    private Material _invalidMaterial;

    public void Initialize(
        TrapBlueprint blueprint,
        Camera cam,
        GhostMaterialSettings materials
        )
    {
        _blueprint = blueprint;
        _cam = cam;
        
        _validMaterial = materials.validMaterial;
        _invalidMaterial = materials.invalidMaterial;
        
        _renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        UpdateAnchor();
        UpdateMaterial();
    }

    private void UpdateAnchor()
    {
        var ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit))
        {
            _anchor = null;
            return;
        }
        
        var anchor = hit.collider.GetComponent<TrapAnchor>();

        if (!anchor)
        {
            _anchor = null;
            return;
        }
        
        _anchor = anchor;
        
        transform.position = anchor.transform.position;
        transform.rotation = anchor.transform.rotation;
    }

    private void UpdateMaterial()
    {
        var valid = CanPlace();
        
        var mat = valid ? _validMaterial : _invalidMaterial;
        
        foreach (var r in _renderers) r.material = mat;
    }

    public bool CanPlace()
    {
        if (!_anchor) return false;
        
        if (!_anchor.CanPlaceTrap()) return false;

        return ResourceManager.Instance.CanAfford(
            _blueprint.stoneCost,
            _blueprint.woodCost
        );
    }
    
    public TrapAnchor GetAnchor() => _anchor;
}